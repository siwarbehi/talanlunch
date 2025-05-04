using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;
using TalanLunch.Application.Configurations;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Jobs;
using TalanLunch.Application.Services;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Infrastructure.Repos;
using TalanLunch.API.Hubs;



namespace TalanLunch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // creation de l app
            var builder = WebApplication.CreateBuilder(args);

            // Création du répertoire pour les téléchargements
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            // Ajout des services CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:5174"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            builder.Services.AddSignalR();
            // Ajouter les services à l'injection de dépendances
            builder.Services.AddControllers();

            // Enregistre MediatR
            builder.Services.AddMediatR(cfg =>
     cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));



            // Configuration de la base de données
            builder.Services.AddDbContext<TalanLunchDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Ajouter Swagger pour la documentation de l'API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enregistrement des repositories et services
            builder.Services.AddScoped<IDishRepository, DishRepository>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IMenuRepository, MenuRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Enregistrement du service de mail
            builder.Services.AddTransient<Application.Interfaces.IMailService, Application.Services.MailService>();

            // Ajouter MailSettings à l'injection de dépendances
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            // Configuration des options JSON
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Configuration de l'authentification JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AppSettings:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
                        ValidateIssuerSigningKey = true
                    };

                  
                });

            // Quartz
            builder.Services.AddQuartz(q =>
            {
                var jobKey = new JobKey("ResetMenuOfTheDayJob");

                q.AddJob<ResetMenuOfTheDayJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("ResetMenuOfTheDayTrigger")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(00, 24))
                );
            });
            builder.Services.AddQuartzHostedService();

          
            builder.Services.AddSignalR();

            // Création de l'application
            var app = builder.Build();



         //app.Use...Ajouter des middlewares 

            // CORS
            app.UseCors("AllowReactApp");

            //fichiers statiques
            app.UseStaticFiles();

            // Swagger doc && swaggerUI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //redirige les requête vers https
            app.UseHttpsRedirection();

            
            app.UseAuthentication();
            app.UseAuthorization();

            //routage des controleurs
            app.MapControllers();

            // AJOUT : MapHub pour SignalR
            app.MapHub<NotificationHub>("/notificationHub");
            // <--- à adapter selon ton Hub (ex: NotificationHub)

            app.Run();
        }
    }
}

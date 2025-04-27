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
using talanlunch.Application.Hubs;

namespace TalanLunch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cr�ation du r�pertoire pour les t�l�chargements
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
            // Ajouter les services � l'injection de d�pendances
            builder.Services.AddControllers();

            // Configuration de la base de donn�es
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

            // Ajouter MailSettings � l'injection de d�pendances
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            // Configuration des options JSON
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

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
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(17, 58))
                );
            });
            builder.Services.AddQuartzHostedService();

            // AJOUT : Enregistrement SignalR
            builder.Services.AddSignalR();

            // Cr�ation de l'application
            var app = builder.Build();

            // Activation de CORS
            app.UseCors("AllowReactApp");

            app.UseStaticFiles();

            // Configure le pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // AJOUT : Important avant Authorization
            app.UseAuthorization();

            app.MapControllers();

            // AJOUT : MapHub pour SignalR
            app.MapHub<NotificationHub>("/notificationHub"); // <--- � adapter selon ton Hub (ex: NotificationHub)

            app.Run();
        }
    }
}

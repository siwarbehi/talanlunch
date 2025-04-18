using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Services;
using TalanLunch.Application.Configurations;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using TalanLunch.Application.Jobs;





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

            // Ajouter les services par d�faut
            builder.AddServiceDefaults();

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

            // Configuration de l'authentification JWT dans l'application ASP.NET Core
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Active l'authentification avec JWT (JSON Web Token)
                .AddJwtBearer(options =>
                {
                    //  D�finition des param�tres de validation du token JWT
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // V�rifie que l'�metteur (Issuer) du token est valide
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"], // R�cup�re l'issuer d�fini dans appsettings.json

                        ValidateAudience = true, // V�rifie que l'audience (Audience) du token est valide
                        ValidAudience = builder.Configuration["AppSettings:Audience"], // R�cup�re l'audience d�finie dans appsettings.json

                        ValidateLifetime = true, // V�rifie que le token n'a pas expir�

                        // Cl� de signature utilis�e pour v�rifier l'int�grit� du token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)), // R�cup�re la cl� secr�te depuis appsettings.json

                        ValidateIssuerSigningKey = true // V�rifie que le token a �t� sign� avec la cl� correcte
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
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(00, 00)) // Ex�cution � minuit chaque jour
                );

            });
            builder.Services.AddQuartzHostedService();


            // Cr�ation de l'application
            var app = builder.Build();

            // Activation de CORS
            app.UseCors("AllowReactApp");
            // Configurer le middleware pour les fichiers statiques (permettre l'acc�s aux fichiers dans wwwroot)

            app.UseStaticFiles();

            // Configure le pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configuration des middlewares
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Lancer l'application
            app.Run();
        }
    }
}

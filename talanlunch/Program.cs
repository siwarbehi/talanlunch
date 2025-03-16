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




namespace TalanLunch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Création du répertoire pour les téléchargements
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            // Ajouter les services par défaut
            builder.AddServiceDefaults();

            // Ajouter les services à l'injection de dépendances
            builder.Services.AddControllers();

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

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);



            // Enregistrement du service de mail
            builder.Services.AddTransient<Application.Interfaces.IMailService, TalanMailService>();

            // Ajouter MailSettings à l'injection de dépendances
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
                    //  Définition des paramètres de validation du token JWT
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Vérifie que l'émetteur (Issuer) du token est valide
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"], // Récupère l'issuer défini dans appsettings.json

                        ValidateAudience = true, // Vérifie que l'audience (Audience) du token est valide
                        ValidAudience = builder.Configuration["AppSettings:Audience"], // Récupère l'audience définie dans appsettings.json

                        ValidateLifetime = true, // Vérifie que le token n'a pas expiré

                        // Clé de signature utilisée pour vérifier l'intégrité du token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)), // Récupère la clé secrète depuis appsettings.json

                        ValidateIssuerSigningKey = true // Vérifie que le token a été signé avec la clé correcte
                    };
                });


            // Création de l'application
            var app = builder.Build();


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

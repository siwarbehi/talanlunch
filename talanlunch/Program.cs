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
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            


            
            // Enregistrement du service de mail
            builder.Services.AddTransient<Application.Interfaces.IMailService, TalanMailService>();

            // Ajouter MailSettings � l'injection de d�pendances
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            // Configuration des options JSON
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            // Cr�ation de l'application
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

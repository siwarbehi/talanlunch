using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;
using System.Text.Json.Serialization;
using TalanLunch.API.Notifications;
using TalanLunch.Application.Auth.Common;
using TalanLunch.Application.Interfaces;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Infrastructure.Jobs;
using TalanLunch.Infrastructure.Mail;
using TalanLunch.Infrastructure.Repos;
using TalanLunch.API.Hubs;
using System.Security.Claims;


namespace TalanLunch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // creation de l app

            var builder = WebApplication.CreateBuilder(args);

            // Ajout des services CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:5174",
                            "http://localhost:5176"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            builder.Services.AddSignalR();


            // Enregistre MediatR
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));



            // Configuration de la base de donn�es
            builder.Services.AddDbContext<TalanLunchDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Ajouter Swagger pour la documentation de l'API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enregistrement des repositories et services
            builder.Services.AddScoped<IDishRepository, DishRepository>();
            builder.Services.AddScoped<IMenuRepository, MenuRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

            builder.Services.AddScoped<AuthCommon>();

            builder.Services.AddScoped<INotificationSender, SignalRNotificationSender>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Enregistrement du service de mail
            builder.Services.AddTransient<IMailService, MailService>();

            // Ajouter  IOptions<MailSettings>  � l'injection de d�pendances
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            // Configuration des options JSON
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
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
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
               RoleClaimType = ClaimTypes.Role
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
            //permettre � Quartz.NET de d�marrer automatiquement en arri�re-plan
            builder.Services.AddQuartzHostedService();


            builder.Services.AddSignalR();



            // Cr�ation de l'application
            var app = builder.Build();



            //app.Use...Ajouter des middlewares 

            // CORS
            app.UseCors("AllowReactApp");



            // Swagger doc && swaggerUI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //redirige les requ�te vers https
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            //routage des controleurs
            app.MapControllers();

            // AJOUT : MapHub pour SignalR
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}

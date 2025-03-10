
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Application.Interfaces;
using TalanLunch.Infrastructure.Repos;
using TalanLunch.Application.Services;

namespace talanlunch;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        Directory.CreateDirectory(uploadsFolder);
        builder.AddServiceDefaults();
       
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddDbContext<TalanLunchDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IDishRepository, DishRepository>();
        builder.Services.AddScoped<IDishService, DishService>();


        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {  
 
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapScalarApiReference();
            app.MapOpenApi();
        }
       

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

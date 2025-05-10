using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Dishes.Commands.UpdateDish
{
    public class UpdateDishCommand : IRequest<Dish>
    {
        [Required]
        public int DishId { get; set; }

        public IFormFile? Photo { get; set; }

        public string? DishName { get; set; }

        public string? DishDescription { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif.")]
        public decimal? DishPrice { get; set; }

        [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5.")]
        public float? Rating { get; set; }

        // Constructeur vide nécessaire pour le model binding
        public UpdateDishCommand() { }
    }
}

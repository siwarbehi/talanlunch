using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Dtos.Dish
{
  
        public class DishUpdateDto
        {
            public string? DishName { get; set; } 
            public string? DishDescription { get; set; } 

            [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif.")]
            public decimal? DishPrice { get; set; }

            [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5.")]
            public float? Rating { get; set; } // Ajout pour notation (1 à 5)



    }
    
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Dtos.Dish
{
 
        public class DishDto
        {
            [Required]
            public string DishName { get; set; } = string.Empty;

            
            public string DishDescription { get; set; } = string.Empty;



           [Required(ErrorMessage = "Le prix du plat est obligatoire.")]
           public decimal DishPrice { get; set; }


           public IFormFile? DishPhoto { get; set; }
        }
    }


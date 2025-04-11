using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
  
        public class DishUpdateDto
        {
            public string? DishName { get; set; } 
            public string? DishDescription { get; set; } 

            [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être un nombre positif.")]
            public int? DishQuantity { get; set; } 

            [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif.")]
            public decimal? DishPrice { get; set; } 

            public bool? IsSalad { get; set; } 

        }
    
}

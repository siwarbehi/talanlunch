using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
 
        public class DishDto
        {
            [Required]
            public string DishName { get; set; } = string.Empty;

            [Required]
            public string DishDescription { get; set; } = string.Empty;

            [Required]
            public int DishQuantity { get; set; }

            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
            public decimal DishPrice { get; set; }

            [Required]
            public bool IsSalad { get; set; }

            public IFormFile? DishPhoto { get; set; }
        }
    }


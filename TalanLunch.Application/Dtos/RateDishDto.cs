using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class RateDishDto
    {
        public int DishId { get; set; }
        public float Rating { get; set; } // Entre 1 et 5
    }
}

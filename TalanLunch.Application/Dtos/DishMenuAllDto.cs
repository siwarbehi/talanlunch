using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class DishMenuAllDto
    {
        public int DishQuantity { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }

        public string? DishPhoto { get; set; }
        public string? DishDescription { get; set; }




    }
}

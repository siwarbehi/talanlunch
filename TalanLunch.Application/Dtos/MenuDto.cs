using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{

    public class MenuDto
    {
        public string? MenuDescription { get; set; }
        public List<int>? DishIds { get; set; } 

        public MenuDto()
        {
            DishIds = new List<int>();
        }
    }
}
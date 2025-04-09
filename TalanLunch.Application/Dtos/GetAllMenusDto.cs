using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Dtos { 
    public class GetAllMenusDto { 
        public int MenuId { get; set; } 
        public string MenuDescription { get; set; }
        public List<DishMenuAllDto> Dishes { get; set; }

         public bool IsMenuOfTheDay { get; set; } = false;

    }
} 

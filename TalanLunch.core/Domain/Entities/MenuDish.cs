using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.core.Domain.Entities;

namespace TalanLunch.Core.Domain.Entities
{
    public class MenuDish
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}

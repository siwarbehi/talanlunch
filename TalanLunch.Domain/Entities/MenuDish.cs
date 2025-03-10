
using System.ComponentModel.DataAnnotations;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Domain.Entities
{
    public class MenuDish
    {
        public int MenuId { get; set; }
     
        public required Menu Menu { get; set; }

        public int DishId { get; set; }
       
        public required Dish Dish { get; set; }
    }
}

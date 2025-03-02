
using TalanLunch.Core.Domain.Entities;

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

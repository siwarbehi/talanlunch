namespace TalanLunch.Domain.Entities
{
    public class MenuDish
    {
        public int MenuId { get; set; }
        public int DishId { get; set; }
        public int DishQuantity { get; set; }

        public Menu? Menu { get; set; } 
        public Dish? Dish { get; set; }

    }
} 

﻿
namespace TalanLunch.Domain.Entities
{
    public class OrderDish
    {
        public int OrderId { get; set; }
        public required Order Order { get; set; }  
        public int DishId { get; set; }
        public required Dish Dish { get; set; }
        public int Quantity { get; set; }  // Quantité de chaque plat dans la commande
    }
}


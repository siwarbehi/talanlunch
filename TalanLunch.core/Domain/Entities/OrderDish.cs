﻿using TalanLunch.core.Domain.Entities;
using TalanLunch.Core.Domain.Entities;

namespace TalanLunch.Core.Domain.Entities
{
    public class OrderDish
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}


﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TalanLunch.Domain.Entites
{
    public class Dish
    {
        public int DishId { get; set; }
        public string DishName { get; set; } = string.Empty;
        public string DishDescription { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
  
        public int DishQuantity { get; set; }
        public int ReviewCount { get; set; }
        public byte[] DishPhoto { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal DishPrice { get; set; }
        public int CurrentRating { get; set; }
        public bool IsSalad { get; set; }



        public ICollection<MenuDish> MenuDishes { get; set; }
        public ICollection<OrderDish> OrderDishes { get; set; }




    }
}
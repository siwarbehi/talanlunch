using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalanLunch.Domain.Entities
{
    public class Dish
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DishId { get; set; }

        [Required]
        public string DishName { get; set; } = string.Empty;
        public string? DishDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public int ReviewCount { get; set; }
        public string? DishPhoto { get; set; }

        [Required]

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? DishPrice { get; set; }
        public float CurrentRating { get; set; }

        [Required]
        public ICollection<MenuDish> MenuDishes { get; set; } = [];
        [Required]
        public ICollection<OrderDish> OrderDishes { get; set; } = [];

    }
}
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
        [Range(0, int.MaxValue, ErrorMessage = "La quantité ne peut pas être négative.")]

        [Required]
        public int DishQuantity { get; set; }
        [Required]
        public int ReviewCount { get; set; }
        public string? DishPhoto { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]

        public decimal DishPrice { get; set; }
        public int CurrentRating { get; set; }
        public bool IsSalad { get; set; }


        [Required]
        public ICollection<MenuDish> MenuDishes { get; set; } = [];
        [Required]
        public ICollection<OrderDish> OrderDishes { get; set; } = [];




    }
}
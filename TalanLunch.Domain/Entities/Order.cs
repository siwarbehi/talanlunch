using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalanLunch.Domain.Entities;


namespace TalanLunch.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,3)")]
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; } 

        public bool Served { get; set; } 

        public string? OrderRemark { get; set; }

        public int UserId { get; set; }

    
        public required User User { get; set; }
        [Required]
        public ICollection<OrderDish> OrderDishes { get; set; } = [];
    }
}


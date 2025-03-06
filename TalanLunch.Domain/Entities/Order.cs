using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalanLunch.Domain.Entites;


namespace TalanLunch.Domain.Entites
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; } 

        public bool Served { get; set; } 

        public string? OrderRemark { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderDish> OrderDishes { get; set; }
    }
}


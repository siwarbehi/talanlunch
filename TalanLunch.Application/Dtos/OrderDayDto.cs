using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class OrderDayDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public int OrderId { get; set; }

        public string? OrderRemark { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public bool Served { get; set; }
        public DateTime OrderDate { get; set; }
        public List<DishOrderDto> Dishes { get; set; } = new();
    }
}

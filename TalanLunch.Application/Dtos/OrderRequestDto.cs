using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class OrderRequestDto
    {
        public int UserId { get; set; }
        public List<DishOrderDto> Dishes { get; set; } = new();
        public string? OrderRemark { get; set; }             
    }
}

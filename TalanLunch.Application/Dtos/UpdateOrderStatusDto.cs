using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public bool? Paid { get; set; }
        public bool? Served { get; set; }
    }
}

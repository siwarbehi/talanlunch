using MediatR;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Order>
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public List<DishOrder> Dishes { get; set; } = new();
        public string? OrderRemark { get; set; }

        public class DishOrder
        {
            public string DishName { get; set; } = string.Empty;
            public int DishId { get; set; }
            public int Quantity { get; set; }
        }
    }
}

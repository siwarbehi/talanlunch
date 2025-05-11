using MediatR;

namespace TalanLunch.Application.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public bool? Paid { get; set; }
        public bool? Served { get; set; }

        public UpdateOrderStatusCommand() { } // requis pour la désérialisation

        public UpdateOrderStatusCommand(int orderId, bool? paid, bool? served)
        {
            OrderId = orderId;
            Paid = paid;
            Served = served;
        }
    }
}

using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Notifications;

namespace TalanLunch.Application.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler
        : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null) return false;

            bool paidChanged = request.Paid == true && !order.Paid;
            bool servedChanged = request.Served == true && !order.Served;

            if (paidChanged) order.Paid = true;
            if (servedChanged) order.Served = true;

            await _orderRepository.UpdateOrderAsync(order);

            if ((paidChanged || servedChanged) && order.UserId != 0)
            {
                var msg = "";
                if (paidChanged) msg += "Votre commande a été payée avec succès.\n";
                if (servedChanged) msg += "Votre commande a été servie ! Bon appétit !\n";

                await _mediator.Publish(new OrderStatusUpdatedNotification
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    Message = msg.Trim()
                }, cancellationToken);
            }

            return true;
        }
    }
}

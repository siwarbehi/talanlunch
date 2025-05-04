// Application/Handlers/OrderHandlers/UpdateOrderStatusCommandHandler.cs
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalanLunch.Application.Commands.Order;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TalanLunch.Application.Hubs;

namespace TalanLunch.Application.Handlers.OrderHandlers
{
    public class UpdateOrderStatusCommandHandler
        : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public UpdateOrderStatusCommandHandler(
            IOrderRepository orderRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _orderRepository = orderRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(
            UpdateOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null) return false;

            bool paidChanged = dto.Paid == true && !order.Paid;
            bool servedChanged = dto.Served == true && !order.Served;

            if (paidChanged) order.Paid = true;
            if (servedChanged) order.Served = true;

            await _orderRepository.UpdateOrderAsync(order);

            if ((paidChanged || servedChanged) && order.UserId != 0)
            {
                var msg = "";
                if (paidChanged) msg += "Votre commande a été payée avec succès.\n";
                if (servedChanged) msg += "Votre commande a été servie ! Bon appétit !\n";

                await _hubContext.Clients
                    .Group(order.UserId.ToString())
                    .SendAsync("ReceiveNotification", new
                    {
                        OrderId = order.OrderId,
                        Message = msg.Trim(),
                        Type = "status-update",
                        Timestamp = DateTime.UtcNow
                    }, cancellationToken);
            }

            return true;
        }
    }
}

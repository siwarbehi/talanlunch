using MediatR;
using Microsoft.AspNetCore.SignalR;
using TalanLunch.Application.Notifications;
using TalanLunch.API.Hubs;

namespace TalanLunch.API.Notifications
{
    public class OrderStatusUpdatedNotificationHandler
        : INotificationHandler<OrderStatusUpdatedNotification>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderStatusUpdatedNotificationHandler(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(OrderStatusUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients
                .Group(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    OrderId = notification.OrderId,
                    Message = notification.Message,
                    Type = "status-update",
                    Timestamp = DateTime.UtcNow
                }, cancellationToken);
        }
    }
}

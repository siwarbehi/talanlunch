using Microsoft.AspNetCore.SignalR;
using TalanLunch.API.Hubs;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Notifications;

namespace TalanLunch.API.Notifications
{
    public class SignalRNotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationSender(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendOrderStatusUpdateAsync(OrderStatusUpdatedNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            var userGroup = notification.UserId.ToString();

            await _hubContext.Clients
                .Group(userGroup)
                .SendAsync("ReceiveNotification", new
                {
                    notification.OrderId,
                    notification.Message,
                    Type = "status-update",
                    Timestamp = DateTime.UtcNow
                }, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

//communication avec SignalR


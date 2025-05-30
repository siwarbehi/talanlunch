using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Notifications
{
    public class OrderStatusUpdatedNotificationHandler : INotificationHandler<OrderStatusUpdatedNotification>
    {
        private readonly INotificationSender _notificationSender;

        public OrderStatusUpdatedNotificationHandler(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public async Task Handle(OrderStatusUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await _notificationSender
                .SendOrderStatusUpdateAsync(notification, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}


//diffusion de notification
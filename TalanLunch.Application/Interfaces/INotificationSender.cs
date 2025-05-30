using TalanLunch.Application.Notifications;

namespace TalanLunch.Application.Interfaces
{
    public interface INotificationSender
    {
        Task SendOrderStatusUpdateAsync(OrderStatusUpdatedNotification notification, CancellationToken cancellationToken);
    }
}

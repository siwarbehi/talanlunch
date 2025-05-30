using MediatR;

namespace TalanLunch.Application.Notifications
{
    public class OrderStatusUpdatedNotification : INotification
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
//Marque une classe comme un événement
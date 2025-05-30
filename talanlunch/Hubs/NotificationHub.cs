using Microsoft.AspNetCore.SignalR;

namespace TalanLunch.API.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId).ConfigureAwait(false);
            }

            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId).ConfigureAwait(false);
            }

            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
    }
}

using Microsoft.AspNetCore.SignalR;

namespace Smart_Dom.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null) // Ensure httpContext is not null
            {
                var userId = httpContext.Request.Query["userId"];

                if (!string.IsNullOrEmpty(userId))
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, userId);
                }
            }

            return base.OnConnectedAsync();
        }
    }
}

using Microsoft.AspNetCore.SignalR;

namespace Smart_Dom.Hubs.ChatRealTime
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}

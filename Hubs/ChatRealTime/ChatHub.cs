using Microsoft.AspNetCore.SignalR;

namespace Smart_Dom.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var messageObj = new
            {
                sender = senderId,
                content = message,
                timestamp,
                type = "text",
                read = false
            };

            // Gửi tới người nhận
            await Clients.User(receiverId).SendAsync("ReceiveMessage", messageObj);

            // Gửi lại người gửi để cập nhật local
            await Clients.User(senderId).SendAsync("ReceiveMessage", messageObj);
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User {userId} connected");
            return base.OnConnectedAsync();
        }
    }
}

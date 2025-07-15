using Microsoft.AspNetCore.SignalR;
using Smart_Dom.Interfaces;
using Smart_Dom.Repositories;
using Smart_Dom.Services;

namespace Smart_Dom.Hubs.ChatRealTime
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageService _chatService;
        private static readonly Dictionary<string, string> _connections = new();

        public ChatHub(IChatMessageService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId) || string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("❌ SendMessage: Missing sender/receiver/message");
                return;
            }
            try
            {
                Console.WriteLine($"📤 [SendMessage] {senderId} → {receiverId}: {message}");

                // Lưu DB hoặc xử lý logic
                await _chatService.SendMessageAsync(senderId, receiverId, message);

                // Gửi tin nhắn tới người nhận
                await Clients.User(receiverId).SendAsync("ReceiveMessage", new
                {
                    senderId,
                    message,
                    sentAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [SendMessage Error] {ex.Message}");
                throw; // Trả lỗi về FE
            }
        }



        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
            {
                Console.WriteLine("❌ HttpContext is null.");
                return base.OnConnectedAsync();
            }

            var userId = httpContext.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                _connections[userId] = Context.ConnectionId;
                Console.WriteLine($"Mapped user {userId} to connection {Context.ConnectionId}");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                _connections.Remove(userId);
                Console.WriteLine($"User {userId} disconnected.");
            }

            return base.OnDisconnectedAsync(exception);
        }
    }


}

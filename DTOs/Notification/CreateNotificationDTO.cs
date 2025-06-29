namespace Smart_Dom.DTOs.Notification
{
    public class CreateNotificationDTO
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; } = null;
        public string? TypeNotify { get; set; }
    }
}

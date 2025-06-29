namespace Smart_Dom.DTOs.Notification
{
    public class NotificationViewDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? TypeNotify { get; set; } // "bill" | "maintenance" | ...

        public string? Title { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Priority { get; set; }

        public bool IsRead { get; set; }
    }
}

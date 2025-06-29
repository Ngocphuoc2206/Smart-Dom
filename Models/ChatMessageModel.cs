using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class ChatMessageModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? ReceiverId { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        public string? ChatSession {  get; set; }
        public bool IsRead { get; set; } = false; // dùng để đánh dấu chưa đọc
    }
}

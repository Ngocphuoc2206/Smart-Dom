using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class NotificationModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [Required] 
        public string? Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}

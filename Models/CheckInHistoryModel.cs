using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class CheckInHistoryModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public RoomModel? Room { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckInTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOutTime { get; set; }
        public string? Status { get; set; } // e.g., "checked-in", "checked-out", "cancelled"
    }
}

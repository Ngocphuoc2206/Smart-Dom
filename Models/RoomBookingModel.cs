using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class RoomBookingModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public RoomModel? Room { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DesiredStart { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DesiredEnd { get; set; }

        [Required]
        public string Status { get; set; } = "pending";

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class MaintenanceRequest
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
        public string IncidentType { get; set; } // e.g., "Điện", "Nước", "Hệ thống HVAC", "Khác"

        [Required]
        public string Location { get; set; } // e.g., "Phòng khách", "Phòng ngủ", "Nhà bếp", "Nhà vệ sinh"

        [Required]
        public string PriorityLevel { get; set; } // e.g., "Thấp", "Trung bình", "Cao"
        
        [Required]
        public string Description { get; set; } // Detailed description of the issue
        
        public string Feedback { get; set; } // Feedback from the maintenance team after resolving the issue
    }
}

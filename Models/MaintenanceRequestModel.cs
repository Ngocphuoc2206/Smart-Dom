using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class MaintenanceRequestModel
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
        public string IncidentType { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string PriorityLevel { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime RequestDate { get; set; }

        public string? ResponeFromOwners { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}

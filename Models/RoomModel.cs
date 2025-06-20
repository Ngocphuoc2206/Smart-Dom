using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class RoomModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public int RoomNumber { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public float Area { get; set; } // in square meters

        [Required]
        public float Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        // 0: Available, 1: Occupied, 2: Under Maintenance
        public string? Status { get; set; }

        [Required]
        public string? Amenities { get; set; } // JSON string for amenities
    }
}

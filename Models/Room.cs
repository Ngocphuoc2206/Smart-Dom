using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class Room
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? RoomNumber { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public int Area { get; set; } // in square meters

        [Required]
        public int Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        // 0: Available, 1: Occupied, 2: Under Maintenance
        public int Status { get; set; }
    }
}

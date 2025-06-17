using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class Contract
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public float DepositAmount { get; set; }

        // e.g., 0:"Active", 1:"Expired", 2:"Cancelled"
        public int Status { get; set; }

        [Required]
        public int IDUser { get; set; }
        [ForeignKey("IDUser")]
        public UserModel? User { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }
    }
}

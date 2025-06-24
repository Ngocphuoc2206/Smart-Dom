using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class ContractModel
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public float DepositAmount { get; set; }

        public string? Status { get; set; }

        [Required]
        public int IDUser { get; set; }
        [ForeignKey("IDUser")]
        public UserModel? User { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public RoomModel? Room { get; set; }
    }
}

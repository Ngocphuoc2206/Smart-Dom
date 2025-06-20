using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class InvoiceModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IDUser { get; set; }
        [ForeignKey("IDUser")]
        public UserModel? User { get; set; }

        [Required]
        public int IDRoom { get; set; }
        [ForeignKey("IDRoom")]
        public RoomModel? Room { get; set; }

        [Required]
        public string InvoiceType { get; set; } // e.g., "Tiền phòng", "Tiền điện", "Tiền nước", "Phí dịch vụ", "Khác" 

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDateLimit { get; set; }

        [Required]
        public float ElectricUsage { get; set; } // in kWh

        [Required]
        public float WaterUsage { get; set; } // in m3

        [Required]
        public float TotalAmount { get; set; } // in VND

        [StringLength(500)]
        public string? Note { get; set; } // Additional notes or comments

        [Required]
        public string Status { get; set; } // e.g., "Chưa thanh toán", "Đã thanh toán", "Quá hạn"

    }
}

using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string? FullName { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        public string? IDCard { get; set; } // Identity card number

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(150)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? EmergencyContact { get; set; }

        [DataType(DataType.PhoneNumber)]

        [StringLength(15)]
        public string? EmergencyPhone { get; set; }
    }
}

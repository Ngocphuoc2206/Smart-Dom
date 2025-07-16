using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class RoomReviewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OverallRating { get; set; }

        [Required]
        public int CleanlinessRating { get; set; }

        [Required]
        public int AmenitiesRating { get; set; }

        [Required]
        public int LocationRating { get; set; }

        [Required]
        public int ValueForMoneyRating { get; set; }

        [Required]
        public int HostAttitudeRating { get; set; }

        [Required]
        public string ExperienceComment { get; set; } = string.Empty;

        [Required]
        public bool IsAnonymous { get; set; }

        [Required]
        public List<ReviewImageModel> Images { get; set; } = new();

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public UserModel? User { get; set; }

        // Không [Required], cho phép null
        [DataType(DataType.Date)]
        public DateTime? ResponseDate { get; set; }

        // Không [Required], cho phép null hoặc chuỗi rỗng
        public string? ResponseFromOwner { get; set; }

        // ✅ CreatedAt là bắt buộc, không cần nullable
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

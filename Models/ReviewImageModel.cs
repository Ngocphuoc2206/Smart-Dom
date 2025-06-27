using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Dom.Models
{
    public class ReviewImageModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public int RoomReviewId { get; set; }
        [ForeignKey("RoomReviewId")]
        public RoomReviewModel RoomReview { get; set; } = null!;
    }
}

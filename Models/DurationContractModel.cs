using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class DurationContractModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int Duration { get; set; } // Duration in months
        [Required]
        public float Price { get; set; } // Price for the duration in currency units
    }
}

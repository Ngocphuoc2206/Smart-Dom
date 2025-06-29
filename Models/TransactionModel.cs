using System.ComponentModel.DataAnnotations;

namespace Smart_Dom.Models
{
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public string? Method { get; set; } // momo, zalopay, bank, etc
        [Required]
        public string? Status { get; set; } // success, fail
        public DateTime PaidAt { get; set; }
        [Required]
        public string? TransactionCode { get; set; }
    }
}

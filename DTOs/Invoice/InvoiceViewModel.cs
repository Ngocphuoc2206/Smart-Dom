namespace Smart_Dom.DTOs.Invoice
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float RoomNumber { get; set; }
        public string? Tenant { get; set; }
        public float RentRooms { get; set; }
        public string? InvoiceType { get; set; }
        public float InvoiceAmount { get; set; } = 0;
        public float ElectricUsage { get; set; } = 0;
        public float WaterUsage { get; set; } = 0;
        public float ServiceFees { get; set; } = 0;
        public string? Note { get; set; }
        public DateTime? InvoiceDateLimit { get; set; }
        public string? Status { get; set; }
    }
}

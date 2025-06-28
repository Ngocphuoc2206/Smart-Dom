namespace Smart_Dom.DTOs.Invoice
{
    public class CreateInvoice
    {
        public int? RoomNumber { get; set; }
        public string? Tenant { get; set; }
        public string? InvoiceType { get; set; } // e.g., "Tiền phòng", "Tiền điện", "Tiền nước", "Phí dịch vụ", "Khác"
        public DateTime InvoiceDateLimit { get; set; } // Deadline for payment
        public float ElectricUsage { get; set; } // in kWh
        public float WaterUsage { get; set; } // in m3
        public float ServiceFees { get; set; }
        public float TotalAmount { get; set; } // in VND
        public string? Note { get; set; } // Additional notes or comments
    }
}

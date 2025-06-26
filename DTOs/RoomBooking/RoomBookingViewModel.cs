namespace Smart_Dom.DTOs.RoomBooking
{
    public class RoomBookingViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? NumberID { get; set; }
        public float price { get; set; }
        public int UserId { get; set; }
        public int RoomNumber { get; set; }
        public int? DepositAmount { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public DateTime DesiredStart { get; set; }
        public DateTime DesiredEnd { get; set; }
        public string? Status { get; set; }
        public string? RoomBookingStatus { get; set; } // Status of the booking (e.g., pending, confirmed, cancelled)
        public int DurationContract { get; set; } // Duration of the contract in months
    }

}

namespace Smart_Dom.DTOs.RoomBooking
{
    public class RoomBookingViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int RoomNumber { get; set; }
        public DateTime DesiredStart { get; set; }
        public DateTime DesiredEnd { get; set; }
        public string? Status { get; set; }
    }

}

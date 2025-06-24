namespace Smart_Dom.DTOs.RoomBooking
{
    public class CreateRoomBookingDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime DesiredStart { get; set; }
        public int ContractDuration { get; set; }
        public string? Notes { get; set; }
    }
}

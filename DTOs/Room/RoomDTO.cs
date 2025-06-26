namespace Smart_Dom.DTOs.Room
{
    public class RoomDTO
    {
        public int ID { get; set; } // Assuming this is the unique identifier for the room
        public string? FullName { get; set; } = null;// Assuming this is the user's full name associated with the room
        public int RoomNumber { get; set; }
        public float Price { get; set; }
        public int Floor { get; set; }
        public float Area { get; set; }
        public string? Available { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // Assuming status is a string, e.g., "Available", "Occupied"
        public string? RoomBookingStatus { get; set; } // Status of the booking (e.g., pending, confirmed, cancelled)
        public List<string>? Amenities { get; set; }
    }
}
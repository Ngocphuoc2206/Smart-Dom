namespace Smart_Dom.DTOs
{
    public class RoomDTO
    {
        public int Number { get; set; }
        public float Price { get; set; }
        public int Floor { get; set; }
        public float Area { get; set; }
        public string? Available { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // Assuming status is a string, e.g., "Available", "Occupied"
        public List<string>? Amenities { get; set; }
    }
}
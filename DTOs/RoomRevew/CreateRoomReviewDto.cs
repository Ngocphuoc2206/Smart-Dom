namespace Smart_Dom.DTOs.RoomRevew
{
    public class CreateRoomReviewDto
    {
        public int UserID { get; set; }
        public int OverallRating { get; set; }
        public int CleanlinessRating { get; set; }
        public int AmenitiesRating { get; set; }
        public int LocationRating { get; set; }
        public int ValueForMoneyRating { get; set; }
        public int HostAttitudeRating { get; set; }
        public string ExperienceComment { get; set; } = string.Empty;
        public bool IsAnonymous { get; set; }
        public List<IFormFile>? Images { get; set; } // Upload ảnh từ client
    }
}

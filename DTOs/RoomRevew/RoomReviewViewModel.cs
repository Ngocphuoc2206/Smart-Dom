namespace Smart_Dom.DTOs.RoomRevew
{
    public class RoomReviewViewModel
    {
        public int ID {  get; set; }
        public string? FullName { get; set; }
        public int RoomNumber { get; set; }
        public int OverallRating { get; set; }
        public int CleanlinessRating { get; set; }
        public int AmenitiesRating { get; set; }
        public int LocationRating { get; set; }
        public int ValueForMoneyRating { get; set; }
        public int HostAttitudeRating { get; set; }
        public string ExperienceComment { get; set; } = string.Empty;
        public bool IsAnonymous { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? ResponseFromOwner { get; set; }
    }
}

namespace Smart_Dom.DTOs.MaintenanceRequest
{
    public class CreateMaintenanceRequestDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string? IncidentType { get; set; }
        public string? Location { get; set; }
        public string? PriorityLevel { get; set; }
        public string? Description { get; set; }
    }
}

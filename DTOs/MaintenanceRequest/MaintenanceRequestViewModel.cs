namespace Smart_Dom.DTOs.MaintenanceRequest
{
    public class MaintenanceRequestViewModel
    {
        public int ID { get; set; }
        public string? IncidentType { get; set; }
        public int? RoomNumber { get; set; }
        public string? Tenant {  get; set; }
        public string? PriorityLevel { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}

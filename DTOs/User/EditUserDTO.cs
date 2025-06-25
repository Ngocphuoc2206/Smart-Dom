namespace Smart_Dom.DTOs.User
{
    public class EditUserDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? NumberID { get; set; }
        public int RoomNumber { get; set; }
        public DateTime DesiredStart { get; set; } // Desired start date for the contract
        public int? DepositAmount { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public int DurationContract { get; set; } // Duration of the contract in months
    }
}

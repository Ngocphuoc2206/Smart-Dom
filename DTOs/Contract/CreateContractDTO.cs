namespace Smart_Dom.DTOs.Contract
{
    public class CreateContractDTO
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending"; // Default status
        public int DepositAmount { get; set; }
        public int DurationContractID { get; set; }
    }
}

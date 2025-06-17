namespace Smart_Dom.DTO
{
    public class AccountDto
    {
        public string FullName { get; set; }
        public string Email { get; set; } // Email == UserName
        public string Phone { get; set; }
        public string Address { get; set; } // Address of the user
        public string IDCard { get; set; } // Identity card number
        public string Gender { get; set; }
        public string Password { get; set; } // Password for login
    }
}

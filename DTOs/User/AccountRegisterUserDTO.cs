namespace Smart_Dom.DTOs.User
{
    public class AccountRegisterUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; } // Email == UserName
        public string Phone { get; set; }
        public DateTime DOB { get; set; } // Date of Birth
        public string Address { get; set; } // Address of the user
        public string IDCard { get; set; } // Identity card number
        public string Gender { get; set; }
        public string Password { get; set; } // Password for login
    }
}

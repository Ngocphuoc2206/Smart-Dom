namespace Smart_Dom.DTOs.User
{
    public class LoginModelUserDTO
    {
        //Email == username
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
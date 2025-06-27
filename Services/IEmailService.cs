namespace Smart_Dom.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}

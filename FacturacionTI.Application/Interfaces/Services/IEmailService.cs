namespace FacturacionTI.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendConfirmationEmailAsync(string toEmail, string confirmationLink);
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task<bool> SendTwoFactorCodeAsync(string toEmail, string code);
    }
}

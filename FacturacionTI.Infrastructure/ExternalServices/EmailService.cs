using FacturacionTI.Application.Interfaces.Services;
using FacturacionTI.Shared.Logging;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace FacturacionTI.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerApp _loggerApp;

        public EmailService(IConfiguration configuration, ILoggerApp loggerApp)
        {
            _configuration = configuration;
            _loggerApp = loggerApp;
        }

        public async Task<bool> SendConfirmationEmailAsync(string toEmail, string confirmationLink)
        {
            var subject = "Confirmar tu cuenta";
            var body = $@"
                <h2>Bienvenido al Sistema de facturación electrónica</h2>
                <p>Por favor confirma tu cuenta haciendo clic en el siguiente enlace:</p>
                <a href='{confirmationLink}'>Confirmar Email</a>
                <p>Si no creaste esta cuenta, puedes ignorar este email.</p>
            ";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Configuración del cliente SMTP
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "25");
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPass = _configuration["EmailSettings:SmtpPass"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                // Crear el cliente SMTP
                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                // Crear el mensaje de correo
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Agregar el destinatario
                mailMessage.To.Add(toEmail);
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("Error sending email", ex);
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            // Crear el contenido del correo electrónico    
            var subject = "Recuperacion de contraseña";
            var body = $@"
                <h2>Recuperación de contraseña</h2>
                <p>Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace:</p>
                <a href='{resetLink}'>Restablecer Contraseña</a>
                <p>Este enlace expirará en 1 hora.</p>
                <p>Si no solicitaste esto, puedes ignorar este email.</p>
            ";

            // Enviar el correo electrónico
            return await SendEmailAsync(toEmail, subject, body);
        }

        public Task<bool> SendTwoFactorCodeAsync(string toEmail, string code)
        {
            throw new NotImplementedException();
        }
    }
}

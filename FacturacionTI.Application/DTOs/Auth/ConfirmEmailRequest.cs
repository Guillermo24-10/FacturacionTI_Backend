namespace FacturacionTI.Application.DTOs.Auth
{
    public class ConfirmEmailRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}

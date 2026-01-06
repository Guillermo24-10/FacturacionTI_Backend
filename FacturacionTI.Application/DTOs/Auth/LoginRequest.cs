namespace FacturacionTI.Application.DTOs.Security
{
    public class LoginRequest
    {
        public string Ruc { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

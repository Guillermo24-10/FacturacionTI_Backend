using System.Text.Json.Serialization;

namespace FacturacionTI.Application.DTOs.Facturacion.Usuario
{
    public class UserDtoResponse
    {
        public string Ruc { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Permisos { get; set; } = new();
        public string PasswordHash { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationToken { get; set; }
    }
}

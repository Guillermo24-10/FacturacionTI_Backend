using System.Text.Json.Serialization;

namespace FacturacionTI.Application.DTOs.Facturacion.Usuario
{
    public class UserDtoResponse
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        public string Ruc { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationToken { get; set; }
    }
}

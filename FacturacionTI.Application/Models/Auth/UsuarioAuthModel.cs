namespace FacturacionTI.Application.Models.Auth
{
    public class UsuarioAuthModel
    {
        public Guid Id { get; set; }
        public string Ruc { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Permisos { get; set; } = new();
    }
}

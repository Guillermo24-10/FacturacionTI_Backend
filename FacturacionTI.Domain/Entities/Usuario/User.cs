namespace FacturacionTI.Domain.Entities.Usuario
{
    public class User
    {
        public Guid Id { get; set; }
        public string Ruc { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool IsActive { get; set; }
        public string Rol { get; set; } = string.Empty;

    }
}

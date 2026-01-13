using System.ComponentModel.DataAnnotations;

namespace FacturacionTI.Application.DTOs.Auth
{
    public class ForgotPasswordRequest
    {        
        public string Email { get; set; } = string.Empty;
    }
}

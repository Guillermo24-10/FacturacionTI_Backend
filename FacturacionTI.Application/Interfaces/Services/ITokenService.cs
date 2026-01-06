using System.Security.Claims;

namespace FacturacionTI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(string Ruc,Guid usuarioId, string email, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
    }
}

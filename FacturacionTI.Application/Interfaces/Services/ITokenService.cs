using FacturacionTI.Application.Common;
using System.Security.Claims;

namespace FacturacionTI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenResult GenerateAccessToken(string Ruc,Guid usuarioId, string email, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
    }
}

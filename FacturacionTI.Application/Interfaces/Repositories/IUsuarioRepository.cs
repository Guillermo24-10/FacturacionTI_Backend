using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Models.Auth;

namespace FacturacionTI.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioAuthModel> ObtenerInfoUsuarioAsync(LoginRequest request);
        Task<bool> GuardarRefreshTokenAsync(Guid usuarioId, string refreshToken);
    }
}

using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;

namespace FacturacionTI.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UserDtoResponse> ObtenerInfoUsuarioAsync(LoginRequest request);
        Task<bool> GuardarRefreshTokenAsync(Guid usuarioId, string refreshToken);
    }
}

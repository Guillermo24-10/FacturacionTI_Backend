using FacturacionTI.Application.Common;
using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Domain.Entities.Usuario;

namespace FacturacionTI.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<UserDtoResponse>> LoginAsync(LoginRequest request);
        Task<String> GenerateTokenAsync(User user);
        //Task<BaseResponse<RefreshTokenResponse>> RefreshTokenAsync(string refreshToken);
    }
}

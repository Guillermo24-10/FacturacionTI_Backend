using FacturacionTI.Application.Common;
using FacturacionTI.Application.DTOs.Auth;
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
        Task<BaseResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<BaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}

using FacturacionTI.Application.DTOs.Auth;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Models.Auth;
using FacturacionTI.Domain.Entities.Usuario;

namespace FacturacionTI.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioAuthModel> ObtenerInfoUsuarioAsync(LoginRequest request);
        Task<bool> GuardarRefreshTokenAsync(Guid usuarioId, string refreshToken);
        Task<User> FindByIdAsync(Guid id);
        Task<bool> ConfirmEmailAsync(Guid userId, string token);
        Task<bool> ResetPasswordAsync(Guid userId, string passwordHash);
        Task<User?> FindByEmailAsync(string email);
        Task SavePasswordResetTokenAsync(Guid userId, string token, DateTime expiration);
        Task<bool> ValidarResetTokenAsync(Guid userId, string token);
        Task InvalidarResetTokenAsync(Guid userId, string token);
    }
}

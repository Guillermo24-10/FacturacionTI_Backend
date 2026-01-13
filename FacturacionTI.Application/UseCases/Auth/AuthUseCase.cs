using FacturacionTI.Application.Common;
using FacturacionTI.Application.DTOs.Auth;
using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Application.Interfaces.Security;
using FacturacionTI.Application.Interfaces.Services;
using FacturacionTI.Domain.Entities.Usuario;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace FacturacionTI.Application.UseCases.Auth
{
    public class AuthUseCase : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthUseCase(IUsuarioRepository usuarioRepository,
                        IPasswordHasher passwordHasher, ITokenService tokenService,
                        IConfiguration configuration, IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<BaseResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var response = new BaseResponse<string>();

            var user = await _usuarioRepository.FindByIdAsync(request.UserId);
            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }

            var confirmar = await _usuarioRepository.ConfirmEmailAsync(request.UserId, request.Token);
            if (!confirmar)
            {
                response.IsSuccess = false;
                response.Message = "Token inválido o email ya confirmado";
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Email confirmado exitosamente";

            return response;
        }

        public async Task<BaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = new BaseResponse<string>();

            var user = await _usuarioRepository.FindByEmailAsync(request.Email);

            if (user == null || !user.EmailConfirmado)
            {
                response.IsSuccess = false;
                response.Message = "Si el email existe, recibirás instrucciones de recuperación, Email enviado";
                return response;
            }

            var token = _tokenService.GenerateRefreshToken();
            var encodedToken = HttpUtility.UrlEncode(token);
            var expiration = DateTime.Now.AddHours(1);

            await _usuarioRepository.SavePasswordResetTokenAsync(user.Id, token, expiration);

            var resetLink = $"{_configuration["AppUrl"]}/auth/reset-password" +
            $"?userId={user.Id}&token={encodedToken}";

            await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);

            response.IsSuccess = true;
            response.Message = "Si el email existe, recibirás instrucciones de recuperación, Email enviado";

            return response;
        }

        public Task<string> GenerateTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserDtoResponse>> LoginAsync(LoginRequest request)
        {
            var response = new BaseResponse<UserDtoResponse>();
            var usuario = await _usuarioRepository.ObtenerInfoUsuarioAsync(request);
            if (usuario == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario o contraseña incorrectos.";
                return response;
            }

            var isPasswordValid = _passwordHasher.Verify(request.Password, usuario.PasswordHash);
            if (!isPasswordValid)
            {
                response.IsSuccess = false;
                response.Message = "Usuario o contraseña incorrectos.";
                return response;
            }

            var token = _tokenService.GenerateAccessToken(usuario.Ruc, usuario.Id, usuario.Email, usuario.Roles);
            response.IsSuccess = true;
            response.Message = "Autenticacion exitosa!";
            response.Data = new UserDtoResponse
            {
                Token = token.Token,
                Ruc = usuario.Ruc,
                Email = usuario.Email,
                Roles = usuario.Roles,
                ExpirationToken = token.Expiration,
                Permisos = usuario.Permisos,
                Username = request.UserName
            };

            return response;
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new BaseResponse<string>();

            var user = await _usuarioRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }

            // Validate the token
            var tokenValido = await _usuarioRepository.ValidarResetTokenAsync(request.UserId, request.Token);

            if (!tokenValido)
            {
                response.IsSuccess = false;
                response.Message = "Token inválido o expirado";
                return response;
            }

            // Hash the new password
            var passwordHash = _passwordHasher.Hash(request.NewPassword);

            // Update the password
            await _usuarioRepository.ResetPasswordAsync(request.UserId, passwordHash);

            // Invalidate the used token
            await _usuarioRepository.InvalidarResetTokenAsync(request.UserId, request.Token);

            response.IsSuccess = true;
            response.Message = "Contraseña restablecida exitosamente";

            return response;
        }
    }
}

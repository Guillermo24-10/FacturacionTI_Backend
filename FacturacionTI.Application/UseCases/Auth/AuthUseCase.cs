using FacturacionTI.Application.Common;
using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Application.Interfaces.Security;
using FacturacionTI.Application.Interfaces.Services;
using FacturacionTI.Domain.Entities.Usuario;

namespace FacturacionTI.Application.UseCases.Auth
{
    public class AuthUseCase : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthUseCase(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public Task<string> GenerateTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserDtoResponse>> LoginAsync(LoginRequest request)
        {
            var response = new BaseResponse<UserDtoResponse>();
            var passBycript = _passwordHasher.Hash("Admin123");
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
    }
}

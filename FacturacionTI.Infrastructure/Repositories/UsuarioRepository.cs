using Dapper;
using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Infrastructure.Persistencia.Dapper;
using FacturacionTI.Shared.Logging;
using System.Data;

namespace FacturacionTI.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        private readonly ILoggerApp _loggerApp;

        public UsuarioRepository(DbConnectionFactory connectionFactory, ILoggerApp loggerApp)
        {
            _connectionFactory = connectionFactory;
            _loggerApp = loggerApp;
        }

        public Task<bool> GuardarRefreshTokenAsync(Guid usuarioId, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDtoResponse> ObtenerInfoUsuarioAsync(LoginRequest request)
        {
            var response = new UserDtoResponse();

            try
            {
                using (var con = _connectionFactory.CrearConexion())
                {
                    var sp = "";
                    var param = new DynamicParameters();
                    param.Add("@Ruc", request.Ruc);
                    param.Add("@Username", request.UserName);
                    param.Add("@Password", request.Password);

                    response = await con.QueryFirstOrDefaultAsync<UserDtoResponse>
                                        (sp, param, commandType: CommandType.StoredProcedure);
                }

                return response!;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("ObtenerInfoUsuarioAsync", ex);
                return null!;
            }
        }
    }
}

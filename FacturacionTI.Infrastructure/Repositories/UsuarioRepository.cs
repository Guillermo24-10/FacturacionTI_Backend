using Dapper;
using FacturacionTI.Application.DTOs.Facturacion.Usuario;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Application.Models.Auth;
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

        public async Task<UsuarioAuthModel> ObtenerInfoUsuarioAsync(LoginRequest request)
        {
            var usuario = new UsuarioAuthModel();

            try
            {
                using (var con = _connectionFactory.CrearConexion())
                {
                    var sp = "SP_VALIDAR_USUARIO";
                    var param = new DynamicParameters();
                    param.Add("@RUC", request.Ruc);
                    param.Add("@USERNAME", request.UserName);
                    //param.Add("@PASSWORD", request.Password);

                    using var multi = await con.QueryMultipleAsync(sp, param, commandType: CommandType.StoredProcedure);

                    //usaurio
                    usuario = await multi.ReadFirstOrDefaultAsync<UsuarioAuthModel>();
                    if (usuario == null)
                        return null!;

                    //roles
                    usuario.Roles = (await multi.ReadAsync<string>()).ToList();

                    //permisos
                    usuario.Permisos = (await multi.ReadAsync<string>()).ToList();
                }

                return usuario!;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("ObtenerInfoUsuarioAsync", ex);
                return null!;
            }
        }
    }
}

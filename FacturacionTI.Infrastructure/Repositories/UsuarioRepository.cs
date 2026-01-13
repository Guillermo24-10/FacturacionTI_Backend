using Dapper;
using FacturacionTI.Application.DTOs.Security;
using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Application.Models.Auth;
using FacturacionTI.Domain.Entities.Usuario;
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

        public async Task<bool> ConfirmEmailAsync(Guid userId, string token)
        {
            try
            {
                using var con = _connectionFactory.CrearConexion();

                const string sql = @"
                            UPDATE TB_USUARIO
                            SET EMAIL_CONFIRMADO = 1,
                                EMAIL_CONFIRMATION_TOKEN = NULL,
                                FECHA_CONFIRMACION = GETDATE()
                            WHERE ID = @UserId
                              AND EMAIL_CONFIRMATION_TOKEN = @Token
                              AND EMAIL_CONFIRMADO = 0";

                var rows = await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Token = token
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("ConfirmEmailAsync", ex);
                return false;
            }
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            try
            {
                using var con = _connectionFactory.CrearConexion();

                const string sql = @"SELECT tu.ID AS [Id]
		                 , tu.RUC AS [Ruc]
			             , tu.USERNAME AS [Username]
			             , tu.PASSWORD AS [Password]
			             , tu.EMAIL AS [Email]
			             , tu.TELEFONO AS [Telephone]
			             , tu.FECHA_REGISTRO AS [FechaRegistro]
			             , tu.ESTADO AS [IsActive]
			             , tu.EMAIL_CONFIRMADO AS [EmailConfirmado]
		            FROM TB_USUARIO tu
                             WHERE tu.EMAIL = @Email";

                return await con.QueryFirstOrDefaultAsync<User>(
                    sql,
                    new { Email = email }
                );
            }
            catch (Exception ex)
            {
                _loggerApp.Error("FindByEmailAsync", ex);
                return null;
            }
        }

        public async Task<User> FindByIdAsync(Guid id)
        {
            var response = new User();

            try
            {
                using var con = _connectionFactory.CrearConexion();

                const string sp = "SP_LISTAR_USUARIO_X_ID";

                var param = new DynamicParameters();
                param.Add("@UserId", id);

                var user = await con.QueryFirstOrDefaultAsync<User>(
                    sp,
                    param,
                    commandType: CommandType.StoredProcedure
                );

                return user!;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("FindByIdAsync", ex);
                return null!;
            }
        }

        public Task<bool> GuardarRefreshTokenAsync(Guid usuarioId, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task InvalidarResetTokenAsync(Guid userId, string token)
        {
            var sql = @"
                    UPDATE TB_USUARIO
                    SET PASSWORD_RESET_TOKEN = NULL,
                        PASSWORD_RESET_EXPIRATION = NULL,
                        SECURITY_STAMP = NEWID()
                    WHERE ID_USUARIO = @UserId
                      AND PASSWORD_RESET_TOKEN = @Token";

            using var con = _connectionFactory.CrearConexion();
            await con.ExecuteAsync(sql, new
            {
                UserId = userId,
                Token = token
            });
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

        public async Task<bool> ResetPasswordAsync(Guid userId, string passwordHash)
        {
            try
            {
                var sql = @"
                UPDATE TB_USUARIO
                SET PASSWORD_HASH = @PasswordHash,
                    PASSWORD_RESET_TOKEN = NULL,
                    PASSWORD_RESET_EXPIRATION = NULL
                WHERE ID = @UserId";

                using var con = _connectionFactory.CrearConexion();
                var rowsAffected = await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    PasswordHash = passwordHash
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("ResetPasswordAsync", ex);
                return false;
            }
        }

        public async Task SavePasswordResetTokenAsync(Guid userId, string token, DateTime expiration)
        {
            using var con = _connectionFactory.CrearConexion();

            try
            {
                const string sql = @"
                UPDATE TB_USUARIO
                SET PASSWORD_RESET_TOKEN = @Token,
                    PASSWORD_RESET_EXPIRATION = @Expiration
                WHERE ID = @UserId";

                await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Token = token,
                    Expiration = expiration
                });
            }
            catch (Exception ex)
            {
                _loggerApp.Error("SavePasswordResetTokenAsync", ex);
            }
        }

        public async Task<bool> ValidarResetTokenAsync(Guid userId, string token)
        {
            try
            {
                var sql = @"
                SELECT COUNT(1)
                FROM TB_USUARIO
                WHERE ID = @UserId
                  AND PASSWORD_RESET_TOKEN = @Token
                  AND PASSWORD_RESET_EXPIRATION > GETDATE()";

                using var con = _connectionFactory.CrearConexion();
                var valido = await con.ExecuteScalarAsync<int>(sql, new
                {
                    UserId = userId,
                    Token = token
                });

                return valido > 0;
            }
            catch (Exception ex)
            {
                _loggerApp.Error("ValidarResetTokenAsync", ex);
                return false;
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FacturacionTI.Infrastructure.Persistencia.Dapper
{
    public class DbConnectionFactory
    {
        public readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("FacturacionConnection")!;
        }

        public IDbConnection CrearConexion() => new SqlConnection(_connectionString);
    }
}

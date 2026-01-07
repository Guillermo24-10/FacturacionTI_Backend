using FacturacionTI.Application.Interfaces.Repositories;
using FacturacionTI.Application.Interfaces.Security;
using FacturacionTI.Application.Interfaces.Services;
using FacturacionTI.Infrastructure.Identity;
using FacturacionTI.Infrastructure.Persistencia.Dapper;
using FacturacionTI.Infrastructure.Repositories;
using FacturacionTI.Shared.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FacturacionTI.Infrastructure.Exntesions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            //JWT Configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(options =>
            {
                configuration.GetSection("JwtSettings").Bind(options);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var settings = jwtSettings.Get<JwtSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings!.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(settings.Secret))
                    };
                });

            services.AddSingleton<DbConnectionFactory>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ILoggerApp, SerilogLogger>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }
    }
}

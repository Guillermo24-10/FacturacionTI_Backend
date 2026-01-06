using FacturacionTI.Application.Interfaces.Services;
using FacturacionTI.Application.UseCases.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace FacturacionTI.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthUseCase>();
            return services;
        }
    }
}

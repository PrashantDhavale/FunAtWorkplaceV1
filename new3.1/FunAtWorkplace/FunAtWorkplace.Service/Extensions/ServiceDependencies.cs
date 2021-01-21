using FunAtWorkplace.Service.Abstractions;
using FunAtWorkplace.Service.Infrastructure.Repositories;
using FunAtWorkplace.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunAtWorkplace.Service.Extensions
{
    public static class ServiceDependencies
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddSingleton<ITdClient, TdClient>();
            //services.AddSingleton<ITdClientService, TdClientService>();

            return services;
        }
    }
}
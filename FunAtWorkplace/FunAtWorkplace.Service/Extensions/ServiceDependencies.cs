using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunAtWorkplace.Service.Extensions
{
    public static class ServiceDependencies
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<Application.Services.IMessageService,Application.Services.MessageService>();
            services.AddTransient<Core.Abstractions.IMessageService,Infrastructure.Repository.MessageService>();
            //services.AddTransient<Core.Abstractions.ITelegram, Infrastructure.Facade.TelegramClient>();


            return services;
        }
    }
}
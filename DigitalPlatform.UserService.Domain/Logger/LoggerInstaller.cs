using DigitalPlatform.UserService.Share.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalPlatform.UserService.Domain.Logger
{
    public static class LoggerInstaller
    {
        public static void AddLogger(this IServiceCollection services)
        {
            services.AddScoped<ILogger, DefaultLogger>();
        }
    }
}

using DigitalPlatform.UserService.Database;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("app");

                //auto migrate
                try
                {
                    logger.LogInformation("Starting Database Migration!");
                    var dbContext = services.GetRequiredService<DatabaseContext>();
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Finished Database Migration!");
                }
                catch (Exception e)
                {
                    logger.LogWarning(e, "An error occurred migrating the DB");
                }
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

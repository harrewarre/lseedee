using System.IO;
using System.Threading.Tasks;
using LSeeDee.Options;
using LSeeDee.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LSeeDee
{
    class Program
    {
        private static IConfiguration Configuration;

        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var clock = scope.ServiceProvider.GetService<Clock>();
                var drive = scope.ServiceProvider.GetService<DiskSpace>();
            }

            // TrayIcon.Create();

            await host.WaitForShutdownAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.SetBasePath(Directory.GetCurrentDirectory());
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    Configuration = configuration.Build();
                })
                .ConfigureServices(services =>
                {
                    services.Configure<DisplayOptions>(Configuration.GetSection("Display"));
                    services.AddLogging();

                    services.AddSingleton<DisplayPort>();
                    services.AddSingleton<Display>();
                    services.AddSingleton<Clock>();
                    services.AddSingleton<DiskSpace>();
                    // services.AddSingleton<SpotifyTicker>();
                });
        }
    }
}
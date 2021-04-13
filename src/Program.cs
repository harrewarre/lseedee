using System;
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
                var display = scope.ServiceProvider.GetService<Clock>();
            }

            await host.WaitForShutdownAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.SetBasePath(Directory.GetCurrentDirectory());
                    configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

                    Configuration = configuration.Build();
                })
                .ConfigureServices(services =>
                {
                    services.Configure<DisplayOptions>(Configuration.GetSection("Display"));
                    services.AddLogging();

                    services.AddSingleton<Display>();
                    services.AddSingleton<Clock>();
                });
        }
    }
}
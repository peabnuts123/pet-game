using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PetGame.Web
{
    public class Program
    {
        // Explicit configuration
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddJsonFile("_secrets.json", optional: true)
            .AddJsonFile($"_secrets.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    string hostProtocol = Environment.GetEnvironmentVariable("HOST_PROTOCOL");
                    if (String.IsNullOrEmpty(hostProtocol))
                    {
                        hostProtocol = "http";
                    }
                    string port = Environment.GetEnvironmentVariable("PORT");
                    if (String.IsNullOrEmpty(port))
                    {
                        port = "5000";
                    }

                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls($"{hostProtocol}://+:{port}");
                });
    }
}

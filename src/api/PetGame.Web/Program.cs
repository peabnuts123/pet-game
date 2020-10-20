using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using PetGame.Config;

namespace PetGame.Web
{
    public class Program
    {
        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration.Base)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void ConfigureHost(IHostBuilder builder) =>
            builder
                .ConfigureAppConfiguration(builder =>
                    builder.AddConfiguration(Configuration.Base))
                .UseSerilog();

        public static void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Host:Port configuration, only use if specified (e.g. in Docker)
            string hostProtocol = Configuration.Base["HOST_PROTOCOL"];
            string port = Configuration.Base["PORT"];
            if (!String.IsNullOrEmpty(port) && !String.IsNullOrEmpty(hostProtocol))
            {
                builder.UseUrls($"{hostProtocol}://+:{port}");
            }

            builder
                .UseStartup<Startup>();
        }

        public static int Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            ConfigureHost(hostBuilder);

            hostBuilder.ConfigureWebHostDefaults(ConfigureWebHost);
            return hostBuilder;
        }
    }
}

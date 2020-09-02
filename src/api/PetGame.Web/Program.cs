using System;
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
        public static void Main(string[] args)
        {
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

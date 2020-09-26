using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PetGame.Config
{
    public class Configuration
    {
        /// <summary>
        /// Path to read config from in AWS Parameter Store (where used)
        /// </summary>
        public static readonly string AWS_PARAMETER_STORE_CONFIGURATION_PATH = "/pet-game/API/Configuration";

        public static readonly string AWS_PARAMETER_STORE_DATA_PROTECTION_PATH = "/pet-game/API/DataProtection";

        // Explicit configuration
        private static IConfiguration _Base { get; set; }

        /// <summary>
        /// Cached reference to Base/General-purpose IConfiguration object
        /// </summary>
        /// <value></value>
        public static IConfiguration Base
        {
            get
            {
                if (_Base == null)
                {
                    // Create general-purpose configuration
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        // Read non-sensitive configuration
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        // Override base configuration with environment specific config
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                        // Read secrets (not in source control)
                        .AddJsonFile($"_secrets.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                        // Override previous configuration with environment variables
                        .AddEnvironmentVariables();

                    if (IsProduction)
                    {
                        // Read config from AWS parameter store (in production only)
                        builder.AddSystemsManager(configureSource =>
                        {
                            configureSource.Path = Configuration.AWS_PARAMETER_STORE_CONFIGURATION_PATH;
                            // Log errors if can't read from parameter store
                            configureSource.OnLoadException += exceptionContext =>
                            {
                                Log.Error(exceptionContext.Exception, "Error loading configuration from Parameter Store.");
                            };
                        });
                    }

                    // Store Configuration (cached/memoized)
                    _Base = builder.Build();
                }

                return _Base;
            }
        }

        /// <summary>
        /// Manual test for isProduction that checks `ASPNETCORE_ENVIRONMENT` equals "production" (case insensitive).
        /// This is only for configuration contexts that don't have access to `IWebHostEnvironment.IsProduction()`,
        /// which is preferred over this.
        /// </summary>
        /// <value></value>
        private static bool IsProduction
        {
            get
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals(Environments.Production, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}

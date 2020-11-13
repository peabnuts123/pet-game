using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetGame.Business;
using PetGame.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Serilog;

namespace PetGame.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration Configuration, IWebHostEnvironment HostEnvironment)
        {
            this.Configuration = Configuration;
            this.HostEnvironment = HostEnvironment;

            // Validate configuration
            string[] requiredConfigValues = new string[] {
                "Auth0:Domain",
                "Auth0:ClientId",
                "Auth0:ClientSecret",
                "DATABASE_URL",
                "WebClient:AbsoluteUrl",
            };
            foreach (string requiredConfig in requiredConfigValues)
            {
                if (string.IsNullOrEmpty(this.Configuration[requiredConfig]))
                {
                    throw new ApplicationException($"Configuration value '{requiredConfig}' is required and cannot be null or empty.");
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ASP.NET
            services.AddControllers()
                .AddJsonOptions((options) =>
                {
                    // Strip nulls from payload
                    // @TODO This might be annoying later?
                    //  In .NET5 you can specify when to omit properties with JsonIgnore
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            // Store data protection keys in AWS (only in production)
            if (this.HostEnvironment.IsProduction())
            {
                services.AddDataProtection()
                    .PersistKeysToAWSSystemsManager(PetGame.Config.Configuration.AWS_PARAMETER_STORE_DATA_PROTECTION_PATH);
            }

            // Services
            services.AddTransient(typeof(ITakingTreeService), typeof(TakingTreeService));
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IItemService), typeof(ItemService));
            services.AddTransient(typeof(ILeaderboardService), typeof(LeaderboardService));
            services.AddTransient(typeof(IGameService), typeof(GameService));

            // DB
            services.AddDbContext<PetGameContext>();

            // CONFIGURATION FOR AUTH
            // > Cookie-based auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            // > Cookie settings
            .AddCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    // Do not redirect to login, just tell the user to go away
                    OnRedirectToLogin = async (context) =>
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Not authorized");
                    }
                };
            })
            // > OIDC configuration
            .AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{Configuration["Auth0:Domain"]}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = Configuration["Auth0:ClientId"];
                options.ClientSecret = Configuration["Auth0:ClientSecret"];

                options.ResponseType = OpenIdConnectResponseType.Code;

                // Configure the scope - Details: https://auth0.com/docs/scopes/openid-connect-scopes
                options.Scope.Add("openid");

                // Set the callback path, so Auth0 will call back to
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString("/api/auth/callback");

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                // OIDC Callbacks @TODO move these into something separate (they contain business logic)
                options.Events = new OpenIdConnectEvents
                {
                    // Tweak OIDC redirect_uri param to point at frontend so cookies work
                    OnRedirectToIdentityProvider = async (context) =>
                    {
                        if (HostEnvironment.IsProduction())
                        {
                            context.ProtocolMessage.RedirectUri = $"{Configuration["WebClient:AbsoluteUrl"]}{options.CallbackPath}";
                        }
                        await Task.CompletedTask;
                    },

                    // Handle login success
                    OnTokenValidated = async (context) =>
                    {
                        string userAuthId = context.SecurityToken.Subject;
                        IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        await userService.Login(userAuthId);
                    },

                    // Handle Logout
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        ILogger<Startup> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                        logger.LogInformation("Event 'OnRedirectToIdentityProviderForSignOut' is firing");

                        var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["WebClient:AbsoluteUrl"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            });
            }

            ILogger<Startup> logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation("Environment Id: {Id}", Configuration["ENVIRONMENT_ID"]);

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

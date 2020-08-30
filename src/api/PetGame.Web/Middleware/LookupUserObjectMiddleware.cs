using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    public class LookupUserObjectMiddleware : IMiddleware
    {
        public static readonly string AUTHENTICATED_USER = "AuthenticatedUser";

        private readonly IUserService userService;
        private readonly ILogger<LookupUserObjectMiddleware> logger;

        public LookupUserObjectMiddleware(IUserService userService, ILogger<LookupUserObjectMiddleware> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // Lookup user object for request
                string userId = context.User.GetSubject();
                User existingUser = this.userService.GetUserById(userId);

                // Make exception for login route to allow users with bad cookie to rectify their login
                if (existingUser == null && context.Request.Path != "/api/auth/login")
                {
                    // Bad state. User is authenticated but does not exist in database (this should not ever happen)
                    this.logger.LogError($"User is authenticated but not found in database. User Id: {userId}. Requested Path: {context.Request.Path}");

                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        Error = $"No user exists with id '{userId}'.",
                    }));
                    return;
                }
                else
                {
                    // Good state. Store user object in HttpContext for processing in Controller
                    context.Items[AUTHENTICATED_USER] = existingUser;
                }
            }

            // Succeed / continue by default
            await next(context);
        }
    }
}

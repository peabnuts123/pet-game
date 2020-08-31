using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    public class Something
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<AuthController> logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public ActionResult<User> Profile()
        {
            User user = HttpContext.Items[LookupUserObjectMiddleware.AUTHENTICATED_USER] as User;

            return Ok(user);
        }

        [HttpGet]
        [Route("login")]
        public async Task Login([FromQuery(Name = "returnUrl")] string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Logout URLs** settings for the app.

                // @TODO don't hard code this - request.referer?
                RedirectUri = "http://localhost:8080"
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly IConfiguration appSettings;

        public AuthController(IUserService userService, ILogger<AuthController> logger, IConfiguration appSettings)
        {
            this.userService = userService;
            this.logger = logger;
            this.appSettings = appSettings;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult<User>> Profile()
        {
            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            if (user == null)
            {
                // Bad state. User is authenticated but does not exist in database (this should not ever happen)
                this.logger.LogError($"Cannot get user profile. User is authenticated but not found in database. User Id: {userAuthId}.");


                return BadRequest(JsonConvert.SerializeObject(new
                {
                    Error = $"No user exists with auth id '{userAuthId}'.",
                }));
            }

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
                RedirectUri = this.appSettings["WebClient:AbsoluteUrl"],
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("debug/all")]
        // [Authorize]
        public ActionResult<IList<User>> debug_GetAllUsers()
        {
            return Ok(this.userService.debug_GetAllUsers());
        }

    }
}
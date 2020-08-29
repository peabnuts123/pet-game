using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("debug/all")]
        public ActionResult<IList<User>> debug_GetAllUsers()
        {
            return Ok(this.userService.debug_GetAllUsers());
        }

        [HttpPost]
        [Route("debug/login")]
        public ActionResult<User> debug_Login([FromBody] LoginModel model)
        {
            return Ok(this.userService.debug_Login(model.Username));
        }

        [HttpGet]
        [Route("{username}")]
        public ActionResult<User> getUserByUsername(string username)
        {
            User user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound($"No user exists with username {username}");
            }
            else
            {
                return Ok(user);
            }
        }

    }
}
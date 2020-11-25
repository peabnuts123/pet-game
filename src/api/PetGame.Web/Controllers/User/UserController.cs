using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetGame.Business;
using PetGame.Common;
using PetGame.Data;

namespace PetGame.Web
{
    [ApiController]
    [Route("api/user")]
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
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult<User>> Profile()
        {
            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            if (user == null)
            {
                // Bad state. User is authenticated but does not exist in database (this should not ever happen)
                this.logger.LogError("Cannot get user profile. User is authenticated but not found in database. User Id: {UserAuthId}.", userAuthId);

                return StatusCode(500, new
                {
                    Error = $"No user exists with auth id '{userAuthId}'.",
                });
            }

            return Ok(user);
        }

        [HttpPatch]
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult<User>> UpdateProfile(UserUpdateProfileDto dto)
        {
            // VALIDATION
            //  - Username
            if (dto.username != null && string.IsNullOrWhiteSpace(dto.username))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.username)] = $"Cannot be blank, or whitespace"
                });
            }


            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            User updatedUser = await this.userService.UpdateUser(user.Id, new User
            {
                // @TODO automapper
                Username = dto.username,
            });

            return Ok(updatedUser);
        }
    }
}
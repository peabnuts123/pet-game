using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetGame.Business;
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
        [Route("debug/all")]
        // [Authorize]
        public ActionResult<IList<User>> debug_GetAllUsers()
        {
            return Ok(this.userService.debug_GetAllUsers());
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


                return BadRequest(JsonConvert.SerializeObject(new
                {
                    Error = $"No user exists with auth id '{userAuthId}'.",
                }));
            }

            return Ok(user);
        }

        [HttpPatch]
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult<User>> UpdateProfile(User dto)
        {
            string userAuthId = HttpContext.User.GetSubject();
            // @TODO one day will have to straighten this whole "2 primary keys" thing
            //  and do less trips to the DB
            User user = await this.userService.GetUserByAuthId(userAuthId);

            try
            {
                User updatedUser = await this.userService.UpdateUser(user.Id, dto);
                return Ok(updatedUser);
            }
            catch (ArgumentException e)
            {
                return BadRequest(JsonConvert.SerializeObject(new
                {
                    Error = e.ToString(),

                }));
            }
        }
    }
}
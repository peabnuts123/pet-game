using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    [ApiController]
    [Route("api/taking-tree")]
    public class TakingTreeController : ControllerBase
    {
        private readonly ITakingTreeService takingTreeService;
        private readonly IUserService userService;

        public TakingTreeController(ITakingTreeService takingTreeService, IUserService userService)
        {
            this.takingTreeService = takingTreeService;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<IList<TakingTreeInventoryItem>> GetAllItems()
        {
            return Ok(this.takingTreeService.GetAllItems());
        }

        [HttpPost]
        [Route("donate")]
        public ActionResult<IList<TakingTreeInventoryItem>> UserDonateItem(TakingTreeDonateItemDto dto)
        {
            // @TODO @DEBUG REMOVE
            string username = HttpContext.Request.Headers["X-Username"];
            if (username == null)
            {
                return BadRequest("Missing debug header X-Username");
            }

            User user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return BadRequest($"No user exists with username '{username}'");
            }
            // </DEBUG>

            this.takingTreeService.UserDonateItem(dto.ItemId, user);
            return Ok(this.takingTreeService.GetAllItems());
        }

        [HttpPost]
        [Route("claim")]
        public ActionResult<IList<Item>> UserClaimItem(TakingTreeClaimItemDto dto)
        {
            // @TODO @DEBUG REMOVE
            string username = HttpContext.Request.Headers["X-Username"];
            if (username == null)
            {
                return BadRequest("Missing debug header X-Username");
            }

            User user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return BadRequest($"No user exists with username '{username}'");
            }
            // </DEBUG>

            this.takingTreeService.UserClaimItem(dto.InventoryItemId, user);
            return Ok(this.takingTreeService.GetAllItems());
        }
    }
}
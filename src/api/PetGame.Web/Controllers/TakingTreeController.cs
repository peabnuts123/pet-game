using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TakingTreeController> logger;

        public TakingTreeController(ITakingTreeService takingTreeService, IUserService userService, ILogger<TakingTreeController> logger)
        {
            this.takingTreeService = takingTreeService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IList<TakingTreeInventoryItem>> GetAllItems()
        {
            return Ok(this.takingTreeService.GetAllItems());
        }

        [HttpPost]
        [Route("donate")]
        [Authorize]
        public async Task<ActionResult<IList<TakingTreeInventoryItem>>> UserDonateItem(TakingTreeDonateItemDto dto)
        {
            string userAuthId = HttpContext.User.GetSubject();

            User user = await this.userService.GetUserByAuthId(userAuthId);
            await this.takingTreeService.UserDonateItem(user, dto.PlayerInventoryItemId);

            return Ok(this.takingTreeService.GetAllItems());
        }

        [HttpPost]
        [Route("claim")]
        [Authorize]
        public async Task<ActionResult<IList<Item>>> UserClaimItem(TakingTreeClaimItemDto dto)
        {
            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);
            await this.takingTreeService.UserClaimItem(user, dto.TakingTreeInventoryItemId);

            return Ok(this.takingTreeService.GetAllItems());
        }
    }
}
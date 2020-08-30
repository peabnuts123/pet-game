using System.Collections.Generic;
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
        public ActionResult<IList<TakingTreeInventoryItem>> UserDonateItem(TakingTreeDonateItemDto dto)
        {
            User user = HttpContext.Items[LookupUserObjectMiddleware.AUTHENTICATED_USER] as User;

            this.takingTreeService.UserDonateItem(dto.PlayerInventoryItemId, user);

            return Ok(this.takingTreeService.GetAllItems());
        }

        [HttpPost]
        [Route("claim")]
        [Authorize]
        public ActionResult<IList<Item>> UserClaimItem(TakingTreeClaimItemDto dto)
        {
            User user = HttpContext.Items[LookupUserObjectMiddleware.AUTHENTICATED_USER] as User;

            this.takingTreeService.UserClaimItem(dto.TakingTreeInventoryItemId, user);

            return Ok(this.takingTreeService.GetAllItems());
        }
    }
}
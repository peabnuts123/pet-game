using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetGame.Business;
using PetGame.Data;

namespace PetGame.Web
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly ILogger<ItemController> logger;

        public ItemController(IItemService itemService, ILogger<ItemController> logger)
        {
            this.itemService = itemService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("debug")]
        public ActionResult<IList<Item>> debug_GetAll()
        {
            return Ok(this.itemService.GetAllItems());
        }
    }
}

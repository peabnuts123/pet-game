using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IList<Item>>> debug_GetAll()
        {
            var allItems = await this.itemService.GetAllItems();
            return Ok(allItems);
        }
    }
}

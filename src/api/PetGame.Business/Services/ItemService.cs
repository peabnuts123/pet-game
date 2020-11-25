using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetGame.Data;

namespace PetGame.Business
{
    public class ItemService : IItemService
    {
        private readonly PetGameContext db;
        private readonly ILogger<ItemService> logger;


        public ItemService(PetGameContext db, ILogger<ItemService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<IList<Item>> GetAllItems()
        {
            return await this.db.Items.ToListAsync();
        }

        public async Task<Item> GetItemById(Guid id)
        {
            return await this.db.Items.SingleOrDefaultAsync((Item item) => item.Id == id);
        }
    }
}

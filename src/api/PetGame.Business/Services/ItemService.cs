using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<Item> GetAllItems()
        {
            return this.db.Items.ToList();
        }

        public Item GetItemById(Guid id)
        {
            return this.db.Items.SingleOrDefault((Item item) => item.Id == id);
        }
    }
}

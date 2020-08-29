using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PetGame.Data;

namespace PetGame.Business
{
    public class ItemService : IItemService
    {
        private readonly ILogger<ItemService> logger;
        private readonly IItemRepository itemRepository;


        public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger)
        {
            this.itemRepository = itemRepository;
            this.logger = logger;
        }

        public IList<Item> GetAllItems()
        {
            return this.itemRepository.GetAll().ToList();
        }

        public Item GetItemById(Guid id)
        {
            return this.itemRepository.GetAll().FirstOrDefault((Item item) => item.Id == id);
        }
    }
}
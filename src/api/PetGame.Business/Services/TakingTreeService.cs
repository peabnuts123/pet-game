using System;
using System.Collections.Generic;
using System.Linq;
using PetGame.Data;

namespace PetGame.Business
{
    public class TakingTreeService : ITakingTreeService
    {
        private readonly IRepository<TakingTreeInventoryItem> takingTreeInventory;
        private readonly IUserService userService;
        private readonly IItemService itemService;

        public TakingTreeService(IRepository<TakingTreeInventoryItem> takingTreeInventory, IUserService userService, IItemService itemService)
        {
            this.takingTreeInventory = takingTreeInventory;
            this.userService = userService;
            this.itemService = itemService;
        }

        public IList<TakingTreeInventoryItem> GetAllItems()
        {
            return this.takingTreeInventory.GetAll().ToList();
        }

        public void UserDonateItem(Guid itemId, User user)
        {
            // Lookup item by ID
            Item item = this.itemService.GetItemById(itemId);

            // Validate item ID
            if (item == null)
            {
                throw new ArgumentException($"Cannot add item to taking tree - no item exists with id '{itemId}'", nameof(itemId));
            }

            // Remove item from user (will throw if invalid)
            this.userService.RemoveItemFromUser(user, itemId);
            // Add item to taking tree
            this.takingTreeInventory.Add(new TakingTreeInventoryItem
            {
                Id = Guid.NewGuid(),
                Item = item,
            });
        }

        public void UserClaimItem(Guid inventoryItemId, User user)
        {
            // Look up / validate item exists in Taking Tree inventory
            TakingTreeInventoryItem existingInventoryItem = this.takingTreeInventory.GetAll()
                .FirstOrDefault((TakingTreeInventoryItem inventoryItem) => inventoryItem.Id == inventoryItemId);

            if (existingInventoryItem == null)
            {
                throw new ArgumentException($"Cannot take item from taking tree - no taking tree inventory item exists with id '{inventoryItemId}'", nameof(inventoryItemId));
            }

            // Remove this item from the taking tree
            this.takingTreeInventory.Remove(existingInventoryItem);
            // Add this item to the user
            this.userService.AddItemToUser(user, existingInventoryItem.Item.Id);
        }
    }
}

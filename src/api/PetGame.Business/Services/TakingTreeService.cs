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

        public void UserDonateItem(Guid playerInventoryItemId, User user)
        {
            // Lookup item by ID
            PlayerInventoryItem playerInventoryItem = this.userService.GetInventoryItemById(user, playerInventoryItemId);

            // Validate item ID
            if (playerInventoryItem == null)
            {
                throw new ArgumentException($"Cannot add item to taking tree - player has no item with id '{playerInventoryItemId}'", nameof(playerInventoryItemId));
            }

            // Remove item from user (will throw if invalid)
            this.userService.RemoveItemFromUser(user, playerInventoryItemId, 1);
            // Add item to taking tree
            this.takingTreeInventory.Add(new TakingTreeInventoryItem
            {
                Id = Guid.NewGuid(),
                Item = playerInventoryItem.Item,
            });
        }

        public void UserClaimItem(Guid takingTreeInventoryItemId, User user)
        {
            // Look up / validate item exists in Taking Tree inventory
            TakingTreeInventoryItem existingInventoryItem = this.takingTreeInventory.GetAll()
                .FirstOrDefault((TakingTreeInventoryItem inventoryItem) => inventoryItem.Id == takingTreeInventoryItemId);

            if (existingInventoryItem == null)
            {
                throw new ArgumentException($"Cannot take item from taking tree - no taking tree inventory item exists with id '{takingTreeInventoryItemId}'", nameof(takingTreeInventoryItemId));
            }

            // Remove this item from the taking tree
            this.takingTreeInventory.Remove(existingInventoryItem);
            // Add this item to the user
            this.userService.AddItemToUser(user, existingInventoryItem.Item.Id);
        }
    }
}

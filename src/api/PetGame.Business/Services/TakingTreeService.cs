using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Common;
using PetGame.Data;

namespace PetGame.Business
{
    public class TakingTreeService : ITakingTreeService
    {
        private readonly PetGameContext db;
        private readonly IUserService userService;
        private readonly IItemService itemService;

        public TakingTreeService(PetGameContext db, IUserService userService, IItemService itemService)
        {
            this.db = db;
            this.userService = userService;
            this.itemService = itemService;
        }

        public async Task<IList<TakingTreeInventoryItem>> GetAllItems()
        {
            // @TODO sometimes this includes item.DonatedBy.Inventory - I don't want this
            return await this.db.TakingTreeInventoryItems
                .Include(item => item.Item)
                .Include(item => item.DonatedBy)
                .ToListAsync();
        }

        public async Task UserDonateItem(User user, Guid playerInventoryItemId)
        {
            // @TODO less trips to the database

            // Fetch and validate playerInventoryItemId
            PlayerInventoryItem playerInventoryItem = this.userService.GetInventoryItemById(user, playerInventoryItemId);
            if (playerInventoryItem == null)
            {
                throw new UserCannotDonateException($"Cannot donate item to taking tree. Player has no item with id '{playerInventoryItemId}'", ErrorId.TakingTree_UserCannotDonate_UserDoesNotHaveItem);
            }

            // Remove item from user
            await this.userService.RemoveItemFromUser(user, playerInventoryItemId, 1);
            // Add item to taking tree
            await this.db.TakingTreeInventoryItems.AddAsync(new TakingTreeInventoryItem
            {
                ItemId = playerInventoryItem.Item.Id,
                DonatedById = user.Id,
            });
            await this.db.SaveChangesAsync();
        }

        public async Task UserClaimItem(User user, Guid takingTreeInventoryItemId)
        {
            // Fetch and validate takingTreeInventoryItemId
            TakingTreeInventoryItem existingInventoryItem = await GetTakingTreeInventoryItemById(takingTreeInventoryItemId);
            if (existingInventoryItem == null)
            {
                throw new UserCannotClaimException($"Cannot take item from taking tree. No taking tree inventory item exists with id '{takingTreeInventoryItemId}'", ErrorId.TakingTree_UserCannotClaim_ItemDoesNotExist);
            }

            // Remove this item from the taking tree
            this.db.TakingTreeInventoryItems.Remove(existingInventoryItem);
            await this.db.SaveChangesAsync();

            // Add this item to the user
            await this.userService.AddItemToUser(user, existingInventoryItem.Item.Id, 1);
        }

        public async Task<TakingTreeInventoryItem> GetTakingTreeInventoryItemById(Guid takingTreeInventoryItemId)
        {
            return await this.db.TakingTreeInventoryItems
                .Include(item => item.Item)
                .Include(item => item.DonatedBy)
                .SingleOrDefaultAsync((TakingTreeInventoryItem inventoryItem) => inventoryItem.Id == takingTreeInventoryItemId);
        }

        // EXCEPTIONS
        public class UserCannotDonateException : PetGameException
        {
            public UserCannotDonateException(string message, ErrorId errorId) : base(message, errorId) { }
        }

        public class UserCannotClaimException : PetGameException
        {
            public UserCannotClaimException(string message, ErrorId errorId) : base(message, errorId) { }
        }
    }
}

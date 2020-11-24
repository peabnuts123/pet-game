using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetGame.Data;

namespace PetGame.Business
{
    public class UserService : IUserService
    {
        private readonly PetGameContext db;
        private readonly IItemService itemService;
        private readonly ILogger<UserService> logger;


        public UserService(PetGameContext db, IItemService itemService, ILogger<UserService> logger)
        {
            this.db = db;
            this.itemService = itemService;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieve a User object by their Auth ID
        /// </summary>
        /// <param name="userId">Auth ID for the User</param>
        /// <returns>User object if one exists with this ID, null otherwise</returns>
        public async Task<User> GetUserByAuthId(string userAuthId)
        {
            // @TODO how can we share this `Include()` logic?
            return await this.db.Users
                .Include((user) => user.Inventory)
                .ThenInclude((item) => item.Item)
                .SingleOrDefaultAsync((User user) => user.AuthId == userAuthId);
        }

        /// <summary>
        /// Test whether a given user ID is a valid user that exists
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>Whether the user ID is a valid user that exists</returns>
        public async Task<bool> IsValidUserId(Guid userId)
        {
            return await this.db.Users.AnyAsync((User user) => user.Id == userId);
        }

        /// <summary>
        /// Retrieve a User object by their unique ID
        /// </summary>
        /// <param name="userId">Unique ID for the User</param>
        /// <returns>User object if one exists with this ID, null otherwise</returns>
        public async Task<User> GetUserById(Guid userId)
        {
            return await this.db.Users
                .Include((user) => user.Inventory)
                .ThenInclude((item) => item.Item)
                .SingleOrDefaultAsync((User user) => user.Id == userId);
        }

        /// <summary>
        /// Give the user an item.
        /// </summary>
        /// <param name="user">User for which to give this item</param>
        /// <param name="itemId">Unique ID of the item to give to this user. A PlayerInventoryItem "slot" will be created if one doesn't exist already</param>
        public async Task AddItemToUser(User user, Guid itemId, int amount)
        {
            // Look up item to add (validation)
            Item item = await this.itemService.GetItemById(itemId);
            if (item == null)
            {
                throw new ArgumentException($"Cannot add item to user. No item exists with id '{itemId}'", nameof(itemId));
            }

            // See if player already holds this item
            PlayerInventoryItem existingInventoryItem = GetInventoryItemByItemId(user, itemId);

            if (existingInventoryItem != null)
            {
                // Update inventory item
                existingInventoryItem.Count += amount;
            }
            else
            {
                // Create new inventory item
                PlayerInventoryItem newInventoryItem = new PlayerInventoryItem
                {
                    Count = amount,
                    UserId = user.Id,
                    ItemId = item.Id,
                };
                user.Inventory.Add(newInventoryItem);
            }

            // Save changes to DB
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove a number of an item from this user.
        /// </summary>
        /// <param name="user">User from which to remove this/these item(s)</param>
        /// <param name="playerInventoryItemId">PlayerInventoryItem "slot" from which to remove items</param>
        /// <param name="amount">The amount of this item to remove. Will throw an exception if this is greater than the amount the User currently has</param>
        public async Task RemoveItemFromUser(User user, Guid playerInventoryItemId, int amount)
        {
            PlayerInventoryItem existingInventoryItem = GetInventoryItemById(user, playerInventoryItemId);

            // Validate user has an item with this id
            if (existingInventoryItem == null)
            {
                throw new System.ArgumentException($"Cannot remove item from user. User '{user.Id}' has no inventory item with id '{playerInventoryItemId}'");
            }
            // Validate user has enough of this item to remove
            else if (existingInventoryItem.Count < amount)
            {
                throw new InvalidOperationException($"Cannot remove {amount} of item from user. User '{user.Id}' only has {existingInventoryItem.Count} of item '{playerInventoryItemId}'");
            }

            // Decrease amount
            existingInventoryItem.Count -= amount;

            if (existingInventoryItem.Count == 0)
            {
                // Remove from user entirely
                user.Inventory.Remove(existingInventoryItem);
            }

            // Save changes to DB
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Log the user in. Will create them in the DB if they don't exist
        /// </summary>
        /// <param name="userAuthId">Unique ID for the user</param>
        public async Task Login(string userAuthId)
        {
            User existingUser = await GetUserByAuthId(userAuthId);

            if (existingUser == null)
            {
                await CreateNewUser(userAuthId);
            }
        }

        /// <summary>
        /// Look up a PlayerInventoryItem by ID, for a given User.
        /// </summary>
        /// <param name="user">User to look up this PlayerInventoryItem for</param>
        /// <param name="playerInventoryItemId">Id of the existing PlayerInventoryItem to look up</param>
        /// <returns>PlayerInventoryItem object if one already exists for this User with this ID, null otherwise</returns>
        public PlayerInventoryItem GetInventoryItemById(User user, Guid playerInventoryItemId)
        {
            return user.Inventory.SingleOrDefault((PlayerInventoryItem inventoryItem) => inventoryItem.Id == playerInventoryItemId);
        }

        /// <summary>
        /// Look up a PlayerInventoryItem by the ID of a particular Item, for a given User.
        /// </summary>
        /// <param name="user">User to look up this PlayerInventoryItem for</param>
        /// <param name="itemId">Id of the Item to look up an existing PlayerInventoryItem by</param>
        /// <returns>PlayerInventoryItem object if one already exists for this User with the item with this ID, null otherwise</returns>
        public PlayerInventoryItem GetInventoryItemByItemId(User user, Guid itemId)
        {
            return user.Inventory.SingleOrDefault((PlayerInventoryItem inventoryItem) => inventoryItem.Item.Id == itemId);
        }

        /// <summary>
        /// Given a User object with updated values, look up the the model in the database and
        /// apply valid changes. Note that many fields (e.g. Id, AuthId) cannot be changed.
        /// </summary>
        /// <param name="updatedUser">Updated user object</param>
        /// <returns>The full updated record from the DB</returns>
        public async Task<User> UpdateUser(Guid userId, User updatedUser)
        {
            User existingUser = await GetUserById(userId);

            // Username
            if (updatedUser.Username != null)
            {
                existingUser.Username = updatedUser.Username.Trim();
            }

            // Save user to DB
            this.db.Users.Update(existingUser);
            await this.db.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Create a new User in the Database and populate them with various data.
        /// </summary>
        /// <param name="userAuthId">Unique ID for the new user</param>
        /// <returns>Newly created User object</returns>
        private async Task<User> CreateNewUser(string userAuthId)
        {
            Random rng = new Random();

            // Create new user object
            User newUser = new User
            {
                AuthId = userAuthId,
                Username = $"User {rng.Next(10000, 99999)}",
            };

            // Save new user to DB first
            await this.db.Users.AddAsync(newUser);
            await this.db.SaveChangesAsync();

            // Refresh user from DB
            newUser = await GetUserByAuthId(userAuthId);

            // Add random items to new user
            IList<Item> AllItems = await this.itemService.GetAllItems();
            for (int i = 0; i < rng.Next(4, 7); i++)
            {
                // @TODO don't make a trip to the DB every time
                await this.AddItemToUser(newUser, AllItems[rng.Next(AllItems.Count)].Id, 1);
            }

            return newUser;
        }
    }
}

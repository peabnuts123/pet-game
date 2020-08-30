using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PetGame.Data;

namespace PetGame.Business
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly ILogger<UserService> logger;
        private readonly IItemService itemService;


        public UserService(IRepository<User> userRepository, IItemService itemService, ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.itemService = itemService;
            this.logger = logger;
        }

        /// @TODO @DEBUG Remove.
        public IList<User> debug_GetAllUsers()
        {
            return this.userRepository.GetAll().ToList();
        }

        public void AddItemToUser(User user, Guid itemId)
        {
            // Look up item to add (validation)
            Item item = this.itemService.GetItemById(itemId);
            if (item == null)
            {
                throw new ArgumentException($"Cannot add item to user, no item with id '{itemId}' exists", nameof(itemId));
            }

            // See if player already holds this item
            PlayerInventoryItem existingInventoryItem = GetInventoryItemByItemId(user, itemId);

            if (existingInventoryItem != null)
            {
                // Update inventory item
                existingInventoryItem.Count++;
            }
            else
            {
                // Create new inventory item
                PlayerInventoryItem newInventoryItem = new PlayerInventoryItem
                {
                    Id = Guid.NewGuid(),
                    Item = item,
                    Count = 1,
                    UserId = user.Id,
                    User = user,
                };
                user.Inventory.Add(newInventoryItem);
            }
        }

        public void RemoveItemFromUser(User user, Guid playerInventoryItemId, int amount)
        {
            PlayerInventoryItem existingInventoryItem = GetInventoryItemById(user, playerInventoryItemId);

            // Validate user has an item with this id
            if (existingInventoryItem == null)
            {
                throw new System.ArgumentException($"Cannot remove item from user; User '{user.Id}' has no inventory item with id '{playerInventoryItemId}'");
            }
            // Validate user has enough of this item to remove
            else if (existingInventoryItem.Count < amount)
            {
                throw new InvalidOperationException($"Cannot remove {amount} of item from user; User '{user.Id}' only has {existingInventoryItem.Count} of item '{playerInventoryItemId}'");
            }

            // Decrease amount
            existingInventoryItem.Count -= amount;

            if (existingInventoryItem.Count == 0)
            {
                // Remove from user entirely
                user.Inventory.Remove(existingInventoryItem);
            }
        }

        public void Login(string userId)
        {
            User existingUser = GetUserById(userId);

            if (existingUser == null)
            {
                this.userRepository.Add(CreateNewUser(userId));
            }
        }

        private User CreateNewUser(string userId)
        {
            Random rng = new Random();

            // Create new user object
            User newUser = new User
            {
                Id = userId,
                Username = $"User {rng.Next(10000, 99999)}",
                Inventory = new List<PlayerInventoryItem>(),
            };

            // Add random items to new user
            IList<Item> AllItems = this.itemService.GetAllItems();
            for (int i = 0; i < rng.Next(4, 7); i++)
            {
                this.AddItemToUser(newUser, AllItems[rng.Next(AllItems.Count)].Id);
            }

            return newUser;
        }

        public User GetUserById(string userId)
        {
            return this.userRepository.GetAll().FirstOrDefault((User user) => user.Id == userId);
        }

        public PlayerInventoryItem GetInventoryItemById(User user, Guid playerInventoryItemId)
        {
            return user.Inventory.FirstOrDefault((PlayerInventoryItem inventoryItem) => inventoryItem.Id == playerInventoryItemId);
        }

        private PlayerInventoryItem GetInventoryItemByItemId(User user, Guid itemId)
        {
            return user.Inventory.FirstOrDefault((PlayerInventoryItem inventoryItem) => inventoryItem.Item.Id == itemId);
        }
    }
}
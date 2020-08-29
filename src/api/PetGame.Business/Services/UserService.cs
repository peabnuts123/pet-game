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

        /// @TODO @DEBUG Remove.
        public User debug_Login(string username)
        {
            User existingUser = this.userRepository.GetAll().FirstOrDefault((User existingUser) => existingUser.Username == username);

            if (existingUser == null)
            {
                // Create new user object
                User newUser = new User
                {
                    Inventory = new List<PlayerInventoryItem>(),
                    Username = username,
                };

                // Add random items to new user
                IList<Item> AllItems = this.itemService.GetAllItems();
                Random rng = new Random();
                for (int i = 0; i < rng.Next(4, 7); i++)
                {
                    this.AddItemToUser(newUser, AllItems[rng.Next(AllItems.Count)].Id);
                }

                return this.userRepository.Add(newUser);
            }
            else
            {
                return existingUser;
            }
        }

        public User GetUserByUsername(string username)
        {
            return this.userRepository.GetAll().FirstOrDefault((User user) => user.Username == username);
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
            PlayerInventoryItem existingInventoryItem = GetExistingInventoryItemForUser(user, itemId);

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

        public void RemoveItemFromUser(User user, Guid itemId)
        {
            PlayerInventoryItem existingInventoryItem = GetExistingInventoryItemForUser(user, itemId);

            if (existingInventoryItem == null)
            {
                throw new System.ArgumentException($"Cannot remove item  with id '{itemId}' from user '{user.Username}'. They do not have this item");
            }

            existingInventoryItem.Count--;
            if (existingInventoryItem.Count <= 0)
            {
                // Remove from user entirely
                user.Inventory.Remove(existingInventoryItem);
            }
        }


        private PlayerInventoryItem GetExistingInventoryItemForUser(User user, Guid itemId)
        {
            return user.Inventory.FirstOrDefault((PlayerInventoryItem inventoryItem) => inventoryItem.Item.Id == itemId);
        }
    }
}
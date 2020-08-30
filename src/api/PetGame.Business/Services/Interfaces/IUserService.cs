using System;
using System.Collections.Generic;
using PetGame.Data;

namespace PetGame.Business
{
    public interface IUserService
    {
        /// <summary>
        /// @TODO @DEBUG Remove.
        /// </summary>
        IList<User> debug_GetAllUsers();


        User GetUserById(string userId);
        void AddItemToUser(User user, Guid itemId);
        void RemoveItemFromUser(User user, Guid playerInventoryItemId, int amount);
        PlayerInventoryItem GetInventoryItemById(User user, Guid playerInventoryItemId);
        void Login(string userId);
    }
}
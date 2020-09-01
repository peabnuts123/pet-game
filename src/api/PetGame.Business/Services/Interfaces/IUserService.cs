using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Data;

namespace PetGame.Business
{
    public interface IUserService
    {
        /// <summary>
        /// @TODO @DEBUG Remove.
        /// </summary>
        IList<User> debug_GetAllUsers();


        Task<User> GetUserByAuthId(string userAuthId);
        User GetUserById(Guid userId);
        Task AddItemToUser(User user, Guid itemId, int amount);
        Task RemoveItemFromUser(User user, Guid playerInventoryItemId, int amount);
        Task Login(string userAuthId);
        PlayerInventoryItem GetInventoryItemById(User user, Guid playerInventoryItemId);
        PlayerInventoryItem GetInventoryItemByItemId(User user, Guid itemId);
    }
}
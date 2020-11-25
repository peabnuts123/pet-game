using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Data;

namespace PetGame.Business
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieve a User object by their Auth ID
        /// </summary>
        /// <param name="userId">Auth ID for the User</param>
        /// <returns>User object if one exists with this ID, null otherwise</returns>
        Task<User> GetUserByAuthId(string userAuthId);
        /// <summary>
        /// Test whether a given user ID is a valid user that exists
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>Whether the user ID is a valid user that exists</returns>
        Task<bool> IsValidUserId(Guid userId);
        /// <summary>
        /// Retrieve a User object by their unique ID
        /// </summary>
        /// <param name="userId">Unique ID for the User</param>
        /// <returns>User object if one exists with this ID, null otherwise</returns>
        Task<User> GetUserById(Guid userId);
        /// <summary>
        /// Give the user an item.
        /// </summary>
        /// <param name="user">User for which to give this item</param>
        /// <param name="itemId">Unique ID of the item to give to this user. A PlayerInventoryItem "slot" will be created if one doesn't exist already</param>

        Task AddItemToUser(User user, Guid itemId, int amount);
        /// <summary>
        /// Remove a number of an item from this user.
        /// </summary>
        /// <param name="user">User from which to remove this/these item(s)</param>
        /// <param name="playerInventoryItemId">PlayerInventoryItem "slot" from which to remove items</param>
        /// <param name="amount">The amount of this item to remove. Will throw an exception if this is greater than the amount the User currently has</param>

        Task RemoveItemFromUser(User user, Guid playerInventoryItemId, int amount);
        /// <summary>
        /// Log the user in. Will create them in the DB if they don't exist
        /// </summary>
        /// <param name="userAuthId">Unique ID for the user</param>

        Task Login(string userAuthId);
        /// <summary>
        /// Look up a PlayerInventoryItem by ID, for a given User.
        /// </summary>
        /// <param name="user">User to look up this PlayerInventoryItem for</param>
        /// <param name="playerInventoryItemId">Id of the existing PlayerInventoryItem to look up</param>
        /// <returns>PlayerInventoryItem object if one already exists for this User with this ID, null otherwise</returns>

        PlayerInventoryItem GetInventoryItemById(User user, Guid playerInventoryItemId);
        /// <summary>
        /// Look up a PlayerInventoryItem by the ID of a particular Item, for a given User.
        /// </summary>
        /// <param name="user">User to look up this PlayerInventoryItem for</param>
        /// <param name="itemId">Id of the Item to look up an existing PlayerInventoryItem by</param>
        /// <returns>PlayerInventoryItem object if one already exists for this User with the item with this ID, null otherwise</returns>

        PlayerInventoryItem GetInventoryItemByItemId(User user, Guid itemId);
        /// <summary>
        /// Given a User object with updated values, look up the the model in the database and
        /// apply valid changes. Note that many fields (e.g. Id, AuthId) cannot be changed.
        /// </summary>
        /// <param name="updatedUser">Updated user object</param>
        /// <returns>The full updated record from the DB</returns>

        Task<User> UpdateUser(Guid userId, User updatedUser);
    }
}
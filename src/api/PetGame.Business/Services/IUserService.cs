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
        User debug_Login(string username);
        /// <summary>
        /// @TODO @DEBUG Remove.
        /// </summary>
        IList<User> debug_GetAllUsers();


        User GetUserByUsername(string username);
        void AddItemToUser(User user, Guid itemId);
        void RemoveItemFromUser(User user, Guid itemId);
    }
}
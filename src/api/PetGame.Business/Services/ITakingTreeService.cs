using System;
using System.Collections.Generic;
using PetGame.Data;

namespace PetGame.Business
{
    public interface ITakingTreeService
    {
        IList<TakingTreeInventoryItem> GetAllItems();
        void UserDonateItem(Guid itemId, User user);
        void UserClaimItem(Guid inventoryItemId, User user);
    }
}
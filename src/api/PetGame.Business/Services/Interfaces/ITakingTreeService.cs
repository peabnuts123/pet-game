using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Data;

namespace PetGame.Business
{
    public interface ITakingTreeService
    {
        Task<IList<TakingTreeInventoryItem>> GetAllItems();
        Task UserDonateItem(User user, Guid playerInventoryItemId);
        Task UserClaimItem(User user, Guid takingTreeInventoryItemId);
        Task<TakingTreeInventoryItem> GetTakingTreeInventoryItemById(Guid takingTreeInventoryItemId);

    }
}
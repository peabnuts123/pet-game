using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Data;

namespace PetGame.Business
{
    public interface IItemService
    {
        Task<IList<Item>> GetAllItems();
        Task<Item> GetItemById(Guid id);
    }
}
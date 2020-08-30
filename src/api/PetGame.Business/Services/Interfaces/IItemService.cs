using System;
using System.Collections.Generic;
using PetGame.Data;

namespace PetGame.Business
{
    public interface IItemService
    {
        IList<Item> GetAllItems();
        Item GetItemById(Guid id);
    }
}
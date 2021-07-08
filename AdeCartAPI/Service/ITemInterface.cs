using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public interface ITemInterface
    {
        List<Item> GetItems { get; }
        Item GetItem(string itemName);
        Item GetItemById(int itemId);
        Task AddItem(Item item);
        Task UpdateItem(Item updateItem);
        Task DeleteItem(int itemId);
    }
}

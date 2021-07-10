using AdeCartAPI.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

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

using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AdeCartAPI.Service
{
    public interface IOrderCart
    {
        List<OrderCartData> GetCarts(string userId);
        OrderCartData GetCart(int orderCartId,string userId);
        Task AddCart(string userId);
        Task UpdateCart(OrderCart cart);
    }
}

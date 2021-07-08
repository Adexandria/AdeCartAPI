
using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

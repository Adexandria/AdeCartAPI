
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
        OrderCartData GetCart(OrderCart cart);
        Task AddCart(OrderCart cart);
        Task UpdateCart(OrderCart cart);
    }
}

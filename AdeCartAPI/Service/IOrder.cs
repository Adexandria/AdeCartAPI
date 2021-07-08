using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public interface IOrder
    {
        List<Order> GetOrders(int OrderCartId);
        Order GetOrder(int orderId);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int orderId);
    }
}

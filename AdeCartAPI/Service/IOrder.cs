using AdeCartAPI.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

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

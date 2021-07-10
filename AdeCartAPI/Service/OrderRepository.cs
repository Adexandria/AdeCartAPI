using AdeCartAPI.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public class OrderRepository : IOrder
    {
        readonly SqlService sqlService;
        public OrderRepository(SqlService sqlService)
        {
            this.sqlService=sqlService;
        }
        public async Task AddOrder(Order order)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Order_Insert", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", order.ItemId);
            sqlCommand.Parameters.AddWithValue("OrderCartId", order.OrderCartId);
            sqlCommand.Parameters.AddWithValue("Quantity", order.Quantity);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public async Task DeleteOrder(int orderId)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Order_Delete", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("OrderId", orderId);
            await sqlConnection.OpenAsync();
            sqlCommand.ExecuteNonQuery();
            await  sqlConnection.CloseAsync();
        }

        public Order GetOrder(int orderId)
        {
            var sqlConnection = sqlService.CreateConnection();
            var order = new Order();
            var sqlCommand = new SqlCommand("Order_GetOrder", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("OrderId", orderId);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    order.OrderCartId = Convert.ToInt32(sqlReader["OrderCartId"]);
                    order.ItemId = Convert.ToInt32(sqlReader["ItemId"]);
                    order.OrderId = Convert.ToInt32(sqlReader["OrderId"]);
                    order.Quantity = Convert.ToInt32(sqlReader["Quantity"]);
                }
            }
            sqlConnection.Close();
            return order;
        }

        public List<Order> GetOrders(int OrderCartId)
        {
            var sqlConnection = sqlService.CreateConnection();
            var orders = new List<Order>();
            var sqlCommand = new SqlCommand("Order_GetOrders", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("OrderCartId", OrderCartId);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderCartId = Convert.ToInt32(sqlReader["OrderCartId"]),
                        ItemId = Convert.ToInt32(sqlReader["ItemId"]),
                        OrderId = Convert.ToInt32(sqlReader["OrderId"]),
                        Quantity = Convert.ToInt32(sqlReader["Quantity"])
                    });
                }
            }
            sqlConnection.Close();
            return orders;
        }

        public async  Task UpdateOrder(Order order)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Order_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", order.ItemId);
            sqlCommand.Parameters.AddWithValue("OrderId", order.OrderId);
            sqlCommand.Parameters.AddWithValue("OrderCartId", order.OrderCartId);
            sqlCommand.Parameters.AddWithValue("Quantity", order.Quantity);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }
       
    }
}

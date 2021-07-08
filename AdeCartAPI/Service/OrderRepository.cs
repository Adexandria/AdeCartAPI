using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public class OrderRepository : IOrder
    {
        readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AdeCart;Integrated Security=True;";

        public async Task AddOrder(Order order)
        {
            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Order_Insert", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", order.ItemId);
            sqlCommand.Parameters.AddWithValue("OrderCartId", order.OrderCartId);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public Order GetOrder(int orderId)
        {
            var sqlConnection = CreateConnection();
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
                }
            }
            sqlConnection.Close();
            return order;
        }

        public List<Order> GetOrders(int OrderCartId)
        {
            var sqlConnection = CreateConnection();
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
                        OrderId = Convert.ToInt32(sqlReader["OrderId"])
                    });
                }
            }
            sqlConnection.Close();
            return orders;
        }

        public async  Task UpdateOrder(Order order)
        {
            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Order_Insert", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", order.ItemId);
            sqlCommand.Parameters.AddWithValue("OrderId", order.OrderId);
            sqlCommand.Parameters.AddWithValue("OrderCartId", order.OrderCartId);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}

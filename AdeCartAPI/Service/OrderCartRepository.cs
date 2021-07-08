using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public class OrderCartRepository : IOrderCart
    {
        readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AdeCart;Integrated Security=True;";

        public List<OrderCartData> GetCarts(string userId)
        {
            
                var sqlConnection = CreateConnection();
                var carts = new List<OrderCartData>();
                var sqlCommand = new SqlCommand("OrderCart_GetCarts", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("UserId",userId);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                    carts.Add(new OrderCartData
                    {
                        OrderCartId = Convert.ToInt32(sqlReader["OrderCartId"]),
                        OrderStatus = Convert.ToInt32(sqlReader["OrderStatus"]),
                        UserId = sqlReader["UserId"].ToString()
                    }) ;
                    }
                }
                sqlConnection.Close();
                return carts;
            
        }

        public async Task AddCart(string userId)
        {
            var sqlConnection = CreateConnection();
            var cart = new OrderCart
            {
                UserId = userId
            };
            var sqlCommand = new SqlCommand("OrderCart_Insert", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("UserId", cart.UserId);
            sqlCommand.Parameters.AddWithValue("OrderStatus", Convert.ToInt32(cart.OrderStatus));
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public OrderCartData GetCart(int orderCartId,string userId )
        {
            var sqlConnection = CreateConnection();
            var newCart = new OrderCartData();
            var sqlCommand = new SqlCommand("OrderCart_GetCart", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var orderCart = new OrderCart
            {
                UserId = userId,
                OrderCartId = orderCartId
            };
            sqlCommand.Parameters.AddWithValue("OrderCartId", orderCart.OrderCartId);
            sqlCommand.Parameters.AddWithValue("UserId", orderCart.UserId);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    newCart.OrderCartId = Convert.ToInt32(sqlReader["OrderCartId"]);
                    newCart.OrderStatus = Convert.ToInt32(sqlReader["OrderStatus"]);
                    newCart.UserId = sqlReader["UserId"].ToString();
                }
            }
            sqlConnection.Close();
            return newCart;
        }

        public async Task UpdateCart(OrderCart cart)
        {
            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("OrderCart_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("UserId", cart.UserId);
            sqlCommand.Parameters.AddWithValue("OrderStatus", Convert.ToInt32(cart.OrderStatus));
            sqlCommand.Parameters.AddWithValue("OrderCartId", Convert.ToInt32(cart.OrderCartId));
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

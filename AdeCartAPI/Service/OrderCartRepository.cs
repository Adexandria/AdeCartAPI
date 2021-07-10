using System;
using System.Data;
using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdeCartAPI.Service
{
    public class OrderCartRepository : IOrderCart
    {
        readonly SqlService sqlService;
        public OrderCartRepository(SqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        public List<OrderCartData> GetCarts(string userId)
        {
            try
            {
                var carts = new List<OrderCartData>();

                var sqlConnection = sqlService.CreateConnection();
                var sqlCommand = new SqlCommand("OrderCart_GetCarts", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("UserId", userId);
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
                        });
                    }
                }
                sqlConnection.Close();
                return carts;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task AddCart(string userId)
        {
            try
            {
                var cart = new OrderCart
                {
                    UserId = userId
                };

                var sqlConnection = sqlService.CreateConnection();
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
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public OrderCartData GetCart(int orderCartId,string userId )
        {
            try
            {
                var newCart = new OrderCartData();
                var orderCart = new OrderCart
                {
                    UserId = userId,
                    OrderCartId = orderCartId
                };

                var sqlConnection = sqlService.CreateConnection();
                var sqlCommand = new SqlCommand("OrderCart_GetCart", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
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
            catch (Exception e)
            {

                throw e;
            } 
           
        }

        public async Task UpdateCart(OrderCart cart)
        {
            try
            {
                var sqlConnection = sqlService.CreateConnection();

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
            catch (Exception e)
            {

                throw e;
            }
            
        }
       
    }
}

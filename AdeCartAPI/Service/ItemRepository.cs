using AdeCartAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Service
{
    public class ItemRepository : ITemInterface
    {
        readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AdeCart;Integrated Security=True;";

        public List<Item> GetItems
        {
            get
            {
                var sqlConnection = CreateConnection();
                var items = new List<Item>();
                var sqlCommand = new SqlCommand("Item_GetItems", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure  
                };
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                using (var sqlReader = sqlCommand.ExecuteReader()) 
                {
                    while (sqlReader.Read()) 
                    {
                        items.Add(new Item
                        {   ItemId = Convert.ToInt32(sqlReader["ItemId"]),
                            ItemName = sqlReader["ItemName"].ToString(),
                            ItemDescription = sqlReader["ItemDescription"].ToString(),
                            ItemPrice = Convert.ToInt32(sqlReader["ItemPrice"]),
                            AvailableItem = Convert.ToInt32(sqlReader["AvailableItem"])
                        });
                    }
                }
                sqlConnection.Close();
                return items;
                
            }
        }

        public async Task AddItem(Item item)
        {

            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Item_Insert", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemName", item.ItemName);
            sqlCommand.Parameters.AddWithValue("ItemPrice", item.ItemPrice);
            sqlCommand.Parameters.AddWithValue("ItemDescription", item.ItemDescription);
            sqlCommand.Parameters.AddWithValue("AvailableItem", item.AvailableItem);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public async Task DeleteItem(int itemId)
        {

            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Item_Delete", sqlConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", itemId);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public Item GetItem(string itemName)
        {
            var sqlConnection = CreateConnection();
           
            var sqlCommand = new SqlCommand("Item_GetItem", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemName", itemName);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery(); 
            Item item = null;
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                  while(sqlReader.Read())
                  {
                    item = new Item()
                    {
                        ItemId = Convert.ToInt32(sqlReader["ItemId"]),
                        ItemName = sqlReader["ItemName"].ToString(),
                        ItemDescription = sqlReader["ItemDescription"].ToString(),
                        ItemPrice = Convert.ToInt32(sqlReader["ItemPrice"]),
                        AvailableItem = Convert.ToInt32(sqlReader["AvailableItem"])
                    };
                  }
            }
            sqlConnection.Close();
            return item;

        }
        public Item GetItemById(int itemId)
        {
            var sqlConnection = CreateConnection();

            var sqlCommand = new SqlCommand("Item_GetItemById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", itemId);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            Item item = null;
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    item = new Item()
                    {
                        ItemId = Convert.ToInt32(sqlReader["ItemId"]),
                        ItemName = sqlReader["ItemName"].ToString(),
                        ItemDescription = sqlReader["ItemDescription"].ToString(),
                        ItemPrice = Convert.ToInt32(sqlReader["ItemPrice"]),
                        AvailableItem = Convert.ToInt32(sqlReader["AvailableItem"])
                    };
                }
            }
            sqlConnection.Close();
            return item;

        }
        public async Task UpdateItem(Item updateItem)
        {
            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Item_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("ItemId", updateItem.ItemId);
            sqlCommand.Parameters.AddWithValue("ItemName", updateItem.ItemName);
            sqlCommand.Parameters.AddWithValue("ItemPrice", updateItem.ItemPrice);
            sqlCommand.Parameters.AddWithValue("ItemDescription", updateItem.ItemDescription);
            sqlCommand.Parameters.AddWithValue("AvailableItem", updateItem.AvailableItem);
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

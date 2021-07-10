using System;
using System.Data;
using AdeCartAPI.Model;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdeCartAPI.Service
{
    public class ItemRepository : ITemInterface
    {
        readonly SqlService sqlService;
        public ItemRepository(SqlService sqlService)
        {
            this.sqlService = sqlService;
        }
        
        public List<Item> GetItems
        {
            get
            {
                try
                {
                    var items = new List<Item>();

                    var sqlConnection = sqlService.CreateConnection();
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
                            {
                                ItemId = Convert.ToInt32(sqlReader["ItemId"]),
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
                catch (Exception e)
                {

                    throw e;
                }
             
            }
        }

        public async Task AddItem(Item item)
        {
            try
            {
                var sqlConnection = sqlService.CreateConnection();
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
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task DeleteItem(int itemId)
        {
            try
            { 
                var sqlConnection = sqlService.CreateConnection();
                var sqlCommand = new SqlCommand("Item_Delete", sqlConnection)
                {
                CommandType = System.Data.CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("ItemId", itemId);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
                sqlConnection.Close();
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }

        public Item GetItem(string itemName)
        {
            try
            {
                Item item = new Item();

                var sqlConnection = sqlService.CreateConnection();

                var sqlCommand = new SqlCommand("Item_GetItem", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("ItemName", itemName);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();


                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        item.ItemId = Convert.ToInt32(sqlReader["ItemId"]);
                        item.ItemName = sqlReader["ItemName"].ToString();
                        item.ItemDescription = sqlReader["ItemDescription"].ToString();
                        item.ItemPrice = Convert.ToInt32(sqlReader["ItemPrice"]);
                        item.AvailableItem = Convert.ToInt32(sqlReader["AvailableItem"]);
                    }
                }

                sqlConnection.Close();
                return item;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Item GetItemById(int itemId)
        {
            try
            {
                Item item = new Item();

                var sqlConnection = sqlService.CreateConnection();
                var sqlCommand = new SqlCommand("Item_GetItemById", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("ItemId", itemId);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();


                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        item.ItemId = Convert.ToInt32(sqlReader["ItemId"]);
                        item.ItemName = sqlReader["ItemName"].ToString();
                        item.ItemDescription = sqlReader["ItemDescription"].ToString();
                        item.ItemPrice = Convert.ToInt32(sqlReader["ItemPrice"]);
                        item.AvailableItem = Convert.ToInt32(sqlReader["AvailableItem"]);
                    }
                }

                sqlConnection.Close();
                return item;
            }
            catch (Exception e)
            {

                throw e;
            }
            

        }

        public async Task UpdateItem(Item updateItem)
        {
            try
            {
                var sqlConnection = sqlService.CreateConnection();
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
            catch (Exception e)
            {

                throw e;
            }
           
        }
       
    }
}
    
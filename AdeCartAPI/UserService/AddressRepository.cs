using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AdeCartAPI.Service;
using AdeCartAPI.UserModel;


namespace AdeCartAPI.UserService
{
    public class AddressRepository:IAddress
    {
        readonly SqlService sqlService;
        public AddressRepository(SqlService sqlService)
        {
            this.sqlService = sqlService;
        }
        public async Task CreateAddress(UserAddress userAddress)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Address_Insert", sqlConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("AddressBox1", userAddress.AddressBox1);
            sqlCommand.Parameters.AddWithValue("UserId", userAddress.UserId);
            await sqlConnection.OpenAsync();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
        
        public async Task DeleteAddress(int addressId)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Address_Delete", sqlConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("AddressId", addressId);
            await sqlConnection.OpenAsync();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }


        public async Task UpdateAddress(UserAddress updatedAddress)
        {
            var sqlConnection = sqlService.CreateConnection();
            var sqlCommand = new SqlCommand("Address_Update", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("AddressBox1", updatedAddress.AddressBox1);
            sqlCommand.Parameters.AddWithValue("AddressId", updatedAddress.AddressId);
            sqlCommand.Parameters.AddWithValue("UserId", updatedAddress.UserId);
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }
        public int GetAddress(int addressId) 
        {   
            var sqlConnection = sqlService.CreateConnection();
            var address = new UserAddress();
            var sqlCommand = new SqlCommand("Address_Get", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("AddressId", addressId);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            using (var sqlReader = sqlCommand.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    address.AddressId = Convert.ToInt32(sqlReader["AddressId"]);
                    address.AddressBox1 = sqlReader["AddressBox1"].ToString();
                    address.UserId = sqlReader["UserId"].ToString();
                }
            }
            sqlConnection.Close();
            if(address.AddressId != 0) 
            {
                return address.AddressId;
            }
            return 0;
           
        }
        
        public int GetAddressByUserId(string userId)
        {
            var sqlConnection = sqlService.CreateConnection();
            var address = new UserAddress();
            var sqlCommand = new SqlCommand("Address_GetByUserId", sqlConnection)
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
                    address.AddressId = Convert.ToInt32(sqlReader["AddressId"]);
                    address.AddressBox1 = sqlReader["AddressBox1"].ToString();
                    address.UserId = sqlReader["UserId"].ToString();
                }
            }
            sqlConnection.Close();
            if (address.AddressId != 0)
            {
                return address.AddressId;
            }
            return 0;
        }


        
    }
}

using AdeCartAPI.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.UserService
{
    public class AddressRepository:IAddress
    {
        readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AdeCart;Integrated Security=True;";

        public async Task CreateAddress(UserAddress userAddress)
        {
            var sqlConnection = CreateConnection();
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
        
        public async Task DeleteAddress(Guid addressId)
        {
            var sqlConnection = CreateConnection();
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
            var sqlConnection = CreateConnection();
            var sqlCommand = new SqlCommand("Address_Update", sqlConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("AddressBox1", updatedAddress.AddressBox1);
            sqlCommand.Parameters.AddWithValue("AddressId", updatedAddress.AddressId);
            sqlCommand.Parameters.AddWithValue("UserId", updatedAddress.UserId);
            await sqlConnection.OpenAsync();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        private SqlConnection CreateConnection() 
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}

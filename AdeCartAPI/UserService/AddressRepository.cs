using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AdeCartAPI.UserModel;


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
        
        public async Task DeleteAddress(int addressId)
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
            await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }
        public int GetAddress(int addressId) 
        {
            var sqlConnection = CreateConnection();
            var dataSet = new DataSet();
            var sqlData = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand("Address_Get", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlData.SelectCommand.Parameters.AddWithValue("AddressId", addressId);
            sqlConnection.Open();
            sqlData.SelectCommand.ExecuteNonQuery();
            sqlData.Fill(dataSet);
            sqlConnection.Close();
            return dataSet.ExtendedProperties.Count;
           
        }
        private SqlConnection CreateConnection() 
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace AdeCartAPI.Service
{
    public class SqlService
    {
        string connectionString;
        readonly IConfiguration _config;

        public SqlService(IConfiguration _config)
        {
            this._config = _config;
        }

        public SqlConnection CreateConnection()
        {
            connectionString = _config["ConnectionStrings:AdeCart"];
            var sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}

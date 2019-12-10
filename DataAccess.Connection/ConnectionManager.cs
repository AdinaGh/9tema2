using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Connection
{
    public class ConnectionManager
    {
        private static IDbConnection _connection;

        public static IDbConnection GetConnection()
        {
            if (_connection != null)
            {
                return _connection;
            }

            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            return _connection;
        }
    }
}

using Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class PublisherRepository
    {
        private readonly IDbConnection _connection;
        public PublisherRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int PublisherCount()
        {
            const string sql = @"select count(*) from [Publisher]";
            using (var cmd = new SqlCommand(sql, GetSqlConnection))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Publisher> GetPublishers(int limit)
        {
            var index = 0;
            const string query = "select * from [Publisher]";
            var publishers = new List<Publisher>();
            using (var command = new SqlCommand(query, GetSqlConnection))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while (index < limit && dataReader.Read())
                    {
                        index++;
                        var currentRow = dataReader;
                        var publisher = ReaderToPublisher(currentRow);
                        publishers.Add(publisher);
                    }
                }
            }

            return publishers;
        }

        private Publisher ReaderToPublisher(IDataRecord currentRow)
        {
            return new Publisher
            {
                PublisherId = (int)currentRow["PublisherId"],
                Name = currentRow["Name"].ToString(),
            };
        }

        private SqlConnection GetSqlConnection
        {
            get
            {
                return (SqlConnection)_connection;
            }
        }

    }
}

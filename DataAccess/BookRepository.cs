using Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class BookRepository
    {
        private readonly IDbConnection _connection;

        public BookRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<Book> GetBooks(int? limit = null)
        {
            var index = 0;
            const string query = "select * from [Book]";
            var books = new List<Book>();
            using (var command = new SqlCommand(query, GetSqlConnection))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while ((limit == null || index < limit) && dataReader.Read())
                    {
                        index++;
                        var currentRow = dataReader;
                        var book = ReaderToBook(currentRow);
                        books.Add(book);
                    }
                }
            }
            return books;
        }

        public List<Book> GetAllBooks()
        {
            return GetBooks();
        }

        public List<Book> GetBooksByYear(int year)
        {
            const string query = "select * from [Book] where Year=@Year";
            var books = new List<Book>();
            using (var command = new SqlCommand(query, GetSqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@Year", year));
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var currentRow = dataReader;
                        var book = ReaderToBook(currentRow);
                        books.Add(book);
                    }
                }
            }
            return books;
        }
        public Book GetBookMaxYear()
        {
            var sql = @"select top 1 [year] from [Book] order by [year] desc";
            using (var cmd = new SqlCommand(sql, GetSqlConnection))
            {
                var maxYear = (int)cmd.ExecuteScalar();
                sql = "select * from [Book] where Year=@Year";
                using (var command = new SqlCommand(sql, GetSqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("@Year", maxYear));
                    using (var dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        var currentRow = dataReader;
                        return ReaderToBook(currentRow);
                    }
                }
            }
        }
        private static Book ReaderToBook(IDataRecord currentRow)
        {
            return new Book
            {
                BookId = (int)currentRow["BookId"],
                Title = currentRow["Title"].ToString(),
                PublisherId = currentRow["PublisherId"] as int? ?? 0,
                Year = currentRow["Year"] as int? ?? default,
                Price = currentRow["Price"] as decimal? ?? default
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

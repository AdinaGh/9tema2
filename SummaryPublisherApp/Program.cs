using DataAccess;
using DataAccess.Connection;
using Entities;
using System;
using System.Linq;

namespace SummaryPublisherApp
{
    class Program
    {
        private static PublisherRepository _publisherRepository;
        private static BookRepository _bookRepository;

        static void Main()
        {
            _publisherRepository = new PublisherRepository(ConnectionManager.GetConnection());
            _bookRepository = new BookRepository(ConnectionManager.GetConnection());
            NumberOfRowsFromThePublisher();
            Top10Publishers();
            NumberOfBooksForEachPublisherAndPrice();
        }

        private static void NumberOfRowsFromThePublisher()
        {
            var number = _publisherRepository.PublisherCount();

            Console.WriteLine("Number of rows from the Publisher table:");
            Console.WriteLine(number);
        }

        private static void Top10Publishers()
        {
            var publishers = _publisherRepository.GetPublishers(10);

            Console.WriteLine("Top 10 Publishers:");
            foreach (var publisher in publishers)
            {
                Console.WriteLine(publisher.Display());
            }
        }

        private static void NumberOfBooksForEachPublisherAndPrice()
        {
            var publishers = _publisherRepository.GetPublishers(int.MaxValue);
            var books = _bookRepository.GetBooks(int.MaxValue);

            var list = (from bo in books
                        join pu in publishers on bo.PublisherId equals pu.PublisherId
                        group new { pu, books } by new { pu.PublisherId, pu.Name } into grp
                        select new NumberOfBooksPerPublisher
                        {
                            PublisherName = grp.Key.Name,
                            NoOfBooks = books.Count,
                            TotalPrice = books.Sum((i => i.Price))
                        }).ToList();

            Console.WriteLine("Number of books for each publisher:");
            foreach (var item in list)
            {
                Console.WriteLine(item.Display());
            }
        }
    }
}

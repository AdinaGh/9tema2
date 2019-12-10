using DataAccess;
using DataAccess.Connection;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SummaryBookApp
{
    class Program
    {
        private static BookRepository _bookRepository;

        private static void Main()
        {
            _bookRepository = new BookRepository(ConnectionManager.GetConnection());
            Books2010();
            BookMaxYear();
            Top10Books();
            SaveToFile();
        }

        private static void Books2010()
        {
            var books = _bookRepository.GetBooksByYear(2010);

            Console.WriteLine("2010 Books:");
            foreach (var book in books)
            {
                Console.WriteLine(book.Display());
            }
        }

        private static void BookMaxYear()
        {
            var book = _bookRepository.GetBookMaxYear();

            Console.WriteLine("Book Max Year:");
            Console.WriteLine(book.Display());
        }

        private static void Top10Books()
        {
            var books = _bookRepository.GetBooks(10);

            Console.WriteLine("Top 10 Books:");
            foreach (var book in books)
            {
                Console.WriteLine(book.Display());
            }
        }

        private static void SaveToFile()
        {
            var books = _bookRepository.GetAllBooks();
            SaveToXml(books);
            SaveToJson(books);
        }

        private static void SaveToXml(List<Book> books)
        {
            var path = @"d:\books.xml";

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    var xsSubmit = new XmlSerializer(books.GetType());
                    xsSubmit.Serialize(writer, books);
                    string xml = sww.ToString();

                    File.WriteAllText(path, xml);
                }
            }
            Console.WriteLine($"{path} was created");

        }

        private static void SaveToJson(List<Book> books)
        {
            var path = @"d:\books.json";
            var json = JsonConvert.SerializeObject(books);
            File.WriteAllText(path, json);
            Console.WriteLine($"{path} was created");
        }
    }
}

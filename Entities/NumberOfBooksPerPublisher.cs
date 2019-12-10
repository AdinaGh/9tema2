namespace Entities
{
    public class NumberOfBooksPerPublisher
    {
        public object PublisherName { get; set; }
        public object NoOfBooks { get; set; }
        public object TotalPrice { get; set; }

        public string Display()
        {
            return $"{PublisherName}, Number of Books: {NoOfBooks}, Total Price: {TotalPrice}";
        }
    }
}

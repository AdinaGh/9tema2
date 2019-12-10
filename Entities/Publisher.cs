namespace Entities
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string Display()
        {
            return $"{PublisherId} - {Name}";
        }
    }
}

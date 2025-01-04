namespace Bookstore.DTOs
{
    public class BookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int BookstoreId { get; set; }
    }
}

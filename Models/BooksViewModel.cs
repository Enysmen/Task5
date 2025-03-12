namespace Task5.Models
{
    public class BooksViewModel
    {
        public string Locale { get; set; }
        public int UserSeed { get; set; }
        public double AvgLikes { get; set; }
        public double AvgReviews { get; set; }
        public int PageNumber { get; set; }
        public List<Book> Books { get; set; }
    }
}

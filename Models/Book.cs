namespace Task5.Models
{
    public class Book
    {
        public int Index { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public int Likes { get; set; }
        public int Reviews { get; set; }
        public string CoverUrl { get; set; }
        public string Genre { get; set; }       
        public int Year { get; set; }          
        public string Description { get; set; }

        // Новый список отзывов (каждый отзыв — это пара Автор + Текст).
        public List<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();

    }

    // Модель одного отзыва.
    public class ReviewItem
    {
        public string Author { get; set; }
        public string Text { get; set; }
    }
}

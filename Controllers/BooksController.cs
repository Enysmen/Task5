using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using Task5.Models;
using Task5.Services;

namespace Task5.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookGeneratorService _bookService;

        public BooksController(BookGeneratorService bookService)
        {
            _bookService = bookService;
        }

        // Отображение главной страницы
        public async Task<IActionResult> Index()
        {
            int offset = 0;
            int count = 20; // первая партия: 20 книг
            var books = await _bookService.GenerateBooksAsync("en", 42, offset, count, 3.7, 4.7);
            var model = new BooksViewModel
            {
                Locale = "en",
                UserSeed = 42,
                AvgLikes = 3.7,
                AvgReviews = 4.7,
                PageNumber = 1,
                Books = books
            };
            return View(model);
        }

        // Метод для подгрузки дополнительных страниц (AJAX)
        [HttpGet]
        public async Task<IActionResult> LoadMore(string locale, int userSeed, double avgLikes, double avgReviews, int offset)
        {
            // Если offset равен 0 – первая партия (20 записей), иначе по 10
            int count = (offset == 0) ? 20 : 10;
            var books = await _bookService.GenerateBooksAsync(locale, userSeed, offset, count, avgLikes, avgReviews);
            return PartialView("_BooksPartial", books);
        }

    }
}

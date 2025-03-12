using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;
using Task5.Models;
using Microsoft.Extensions.Configuration;

namespace Task5.Services
{
    public class BookGeneratorService
    {
        private readonly INodeJSService _nodeJSService;
        private readonly GoogleTranslationService _translationService;
        private readonly IConfiguration _configuration;

        public BookGeneratorService(INodeJSService nodeJSService, GoogleTranslationService translationService, IConfiguration configuration)
        {
            _nodeJSService = nodeJSService;
            _translationService = translationService;
            _configuration = configuration;
        }

        public async Task<List<Book>> GenerateBooksAsync(string locale, int userSeed, int offset, int count, double avgLikes, double avgReviews)
        {
            var books = await _nodeJSService.InvokeFromFileAsync<List<Book>>(
                "wwwroot//bookGenerator.cjs",
                args: new object[] { locale, userSeed, offset, count, avgLikes, avgReviews }
            );

          
                // Определяем целевой язык для перевода.
                // Например, для "pt_BR" – "pt", для "pl" – "pl", для "en" – "en".
                string targetLang = locale switch
                {
                    "pt_BR" => "pt",
                    "pl" => "pl",
                    "en" => "en",
                };

                foreach (var book in books)
                {
                    if (!string.IsNullOrWhiteSpace(book.Description))
                    {
                        book.Description = await _translationService.TranslateTextAsync(book.Description, targetLang);
                    }

                    if (book.ReviewItems != null && book.ReviewItems.Count > 0)
                    {
                        // Переводим каждый отзыв
                        for (int i = 0; i < book.ReviewItems.Count; i++)
                        {
                            var review = book.ReviewItems[i];
                            if (!string.IsNullOrWhiteSpace(review.Text))
                            {
                                review.Text = await _translationService.TranslateTextAsync(review.Text, targetLang);
                            }
                        }
                    }
                }
           

            return books;
        }
    }
}

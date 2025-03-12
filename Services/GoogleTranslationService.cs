using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace Task5.Services
{
    public class GoogleTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public GoogleTranslationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Читаем настройки из appsettings.json
            _apiKey = configuration["GoogleTranslation:ApiKey"];
            _endpoint = configuration["GoogleTranslation:Endpoint"]; // например, "https://translation.googleapis.com/language/translate/v2"
        }

        public async Task<string> TranslateTextAsync(string text, string targetLanguage)
        {
            string requestUri = $"{_endpoint}?key={_apiKey}";
            var requestBody = new
            {
                q = text,
                target = targetLanguage,
                format = "text"
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
            string result = await response.Content.ReadAsStringAsync();

            try
            {
                using JsonDocument document = JsonDocument.Parse(result);
                var root = document.RootElement;
                if (root.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("translations", out JsonElement translationsElement) &&
                    translationsElement.GetArrayLength() > 0)
                {
                    var translation = translationsElement[0];
                    if (translation.TryGetProperty("translatedText", out JsonElement translatedTextElement))
                    {
                        return translatedTextElement.GetString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга ответа: {ex.Message}");
            }

            return text;
        }
    }
}

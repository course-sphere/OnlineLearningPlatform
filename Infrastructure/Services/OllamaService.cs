using Application.IServices;
using Domain;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Infrastructure.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _http;
        private readonly AppSettings _appSettings;
        public OllamaService(HttpClient http, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _http = http;
        }

        public async Task<string> GetAIResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false,
                 options = new
                 {
                     num_ctx = 2048,        // giảm context
                     num_predict = 256,     // số token trả về
                     temperature = 0.7
                 }
            };

            //var jsonBody = JsonConvert.SerializeObject(requestBody);
            //var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _http.PostAsJsonAsync(_appSettings.OllamaAI, requestBody);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
            return jsonResponse.response;
        }
    }
}

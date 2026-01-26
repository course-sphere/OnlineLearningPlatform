using Domain.Requests.AI;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AIController : Controller
    {
        private readonly HttpClient _http;

        public AIController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Test([FromBody] AIQuestionRequest request)
        {
            var payload = new
            {
                question = request.Question,
                lesson = new
                {
                    title = "Introduction to Data Science",
                    content = "This lesson covers the origin and history of data science."
                },
                resources = new[]
                {
                    "Video: History of Data Science",
                    "PDF: Data Science Overview"
                }
            };

            var res = await _http.PostAsJsonAsync(
                "http://localhost:5555/ai/guidance",
                payload
            );

            var json = await res.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}

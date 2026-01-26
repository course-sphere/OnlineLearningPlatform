using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLearningPlatform.Controllers
{
    public class OllamaController : Controller
    {
        private readonly IOllamaService _ollamaService;
        public OllamaController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpGet]
        public IActionResult GetAIResponse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAIResponse(string topic)
        {
            var prompt = topic;
            var result = await _ollamaService.GetAIResponseAsync(prompt);
            ViewBag.Result = result;
            return View();
        }
    }
}

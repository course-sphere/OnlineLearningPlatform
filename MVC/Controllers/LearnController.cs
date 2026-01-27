public class LearnController : Controller
{
    public IActionResult Index(Guid id) => Content($"Vào học khóa: {id}"); // Placeholder
}
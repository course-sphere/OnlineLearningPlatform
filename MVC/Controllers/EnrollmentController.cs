using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class EnrollmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

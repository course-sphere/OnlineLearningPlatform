using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AdminController : Controller
    {
        //https://localhost:7276/Admin
        public IActionResult Index()
        {
            return View("Dashboard");
        }

        //https://localhost:7276/Admin/Users
        public IActionResult Users()
        {
            return View();
        }

        //https://localhost:7276/Admin/Payments
        public IActionResult Payments()
        {
            return View();
        }

        //https://localhost:7276/Admin/Reports
        public IActionResult Reports()
        {
            return View();
        }

        //https://localhost:7276/Admin/CourseStatistics
        public IActionResult CourseStatistics()
        {
            return View();
        }
    }
}
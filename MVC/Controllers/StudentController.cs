using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class StudentController : Controller
    {
        // Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // Wallet
        public IActionResult Wallet()
        {
            return View();
        }

        // Payment results
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public IActionResult PaymentFail()
        {
            return View();
        }
    }
}

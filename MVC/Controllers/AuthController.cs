using Application.IServices;
using Domain.Requests.User;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _service.LoginAsync(request);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(request);
            }

            TempData["Success"] = "Login successful!";

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return View(request);
            }
            var result = await _service.RegisterAsync(request);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(request);
            }
            TempData["Success"] = "Register successfully! Please login.";
            return RedirectToAction("Login", "Auth");
        }
    }
}
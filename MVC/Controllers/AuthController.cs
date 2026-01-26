using Application.IServices;
using Domain.Requests.User;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var jwt = result.Result.ToString();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var claims = token.Claims.ToList();

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            TempData["Success"] = "Login successful!";

            return RedirectToAction(nameof(PostLogin));
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
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult PostLogin()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("Index", "Instructor");
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
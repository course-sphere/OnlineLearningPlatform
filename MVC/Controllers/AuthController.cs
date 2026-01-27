using Application.IServices;
using Domain.Requests.User;
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

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request, string? returnUrl = null)
        {
            // 1. Kiểm tra input
            if (!ModelState.IsValid) return View(request);

            // 2. Gọi Service đăng nhập
            var result = await _service.LoginAsync(request);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(request);
            }

            // 3. Giải mã Token lấy Claims
            var jwt = result.Result.ToString();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var claims = token.Claims.ToList();

            // 4. Lưu Cookie đăng nhập
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true }
            );

            TempData["Success"] = "Login successful!";


            // 5. === LOGIC ĐIỀU HƯỚNG THEO ROLE (QUAN TRỌNG) ===

            // Lấy Role từ Claims (Check cả 2 key phổ biến)
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
            var role = roleClaim?.Value;

            // Ưu tiên 1: Nếu là Admin -> Vào trang Admin
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            // Ưu tiên 2: Nếu là Instructor -> Vào trang Giảng viên
            else if (role == "Instructor")
            {
                return RedirectToAction("Index", "Instructor");
            }

            // Ưu tiên 3: Nếu có trang cũ đang truy cập dở -> Quay lại đó (trừ khi trang đó là Login)
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.Contains("Login"))
            {
                return Redirect(returnUrl);
            }

            // Mặc định: Về trang chủ (Student/Guest)
            return RedirectToAction("Index", "Home");
        }

        // ================= REGISTER =================

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

        // ================= LOGOUT =================

        [Authorize] // Phải đăng nhập mới logout được
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }
    }
}
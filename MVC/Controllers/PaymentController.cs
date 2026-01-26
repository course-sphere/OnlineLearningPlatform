using Application;
using Application.IServices;
using Domain.Requests.Payment;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
// using Infrastructure.Libraries; // Bỏ dòng này nếu IUnitOfWork nằm trong Application

namespace MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;
        private readonly IUnitOfWork _unitOfWork; // Biến toàn cục có dấu gạch dưới

        public PaymentController(IPaymentService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork; // Gán tham số vào biến toàn cục
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(Guid courseId)
        {
            var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);

            if (course == null) return NotFound();

            var model = new PaymentCheckoutViewModel
            {
                CourseId = courseId,
                CourseName = course.Title,
                Price = course.Price,
                ImageUrl = course.Image,
                // InstructorName = ... 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(PaymentCheckoutViewModel model)
        {
            var result = await _service.CreatePaymentUrlAsync(new CreateNewPaymentRequest { CourseId = model.CourseId }, HttpContext);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;

                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == model.CourseId);
                if (course != null)
                {
                    model.CourseName = course.Title;
                    model.Price = course.Price;
                    model.ImageUrl = course.Image;
                }

                return View(model);
            }
            if (result.Result == null) return RedirectToAction("Fail");

            return Redirect(result.Result.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var result = await _service.PaymentExecuteAsync(Request.Query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Fail");
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success() => View();
        public IActionResult Fail() => View();
    }
}
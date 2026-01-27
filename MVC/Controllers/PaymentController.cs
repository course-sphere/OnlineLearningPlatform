using Application;
using Application.IServices;
using Domain.Requests.Payment;
using Domain.Responses.Payment; 
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;
        private readonly IEnrollmentService _enrollmentService; // <--- 1. KHAI BÁO THÊM
        private readonly IUnitOfWork _unitOfWork;

        // 2. INJECT VÀO CONSTRUCTOR
        public PaymentController(
            IPaymentService service,
            IEnrollmentService enrollmentService, 
            IUnitOfWork unitOfWork)
        {
            _service = service;
            _enrollmentService = enrollmentService; // <--- Gán giá trị
            _unitOfWork = unitOfWork;
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
            // 1. Xử lý Payment (Validate chữ ký, update DB status = Success)
            var result = await _service.PaymentExecuteAsync(Request.Query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Fail");
            }

            dynamic paymentResponse = result.Result;

            if (paymentResponse != null)
            {
                await _enrollmentService.EnrollStudentAsync((Guid)paymentResponse.CourseId);
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success() => View();
        public IActionResult Fail() => View();
    }
}
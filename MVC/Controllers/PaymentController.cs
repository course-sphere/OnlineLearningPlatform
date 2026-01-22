using Application.IServices;
using Domain.Requests.Payment;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Checkout(Guid courseId)
        {
            var model = new PaymentCheckoutViewModel
            {
                CourseId = courseId,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(PaymentCheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.CreatePaymentUrlAsync(new CreateNewPaymentRequest { CourseId = model.CourseId }, HttpContext);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(model);
            }
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

using Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize] // Bắt buộc phải login mới được Enroll
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> FreeEnroll(Guid courseId)
        {
            // Gọi Service xử lý logic
            var result = await _enrollmentService.EnrollStudentAsync(courseId);

            if (result.IsSuccess)
            {
                // Nếu thành công -> Chuyển hướng sang trang học (Learning Flow)
                // Chúng ta sẽ làm trang này kế tiếp
                return RedirectToAction("Index", "Learn", new { id = courseId });
            }
            else
            {
                // Nếu thất bại (ví dụ khóa có phí mà hack form gửi request)
                // Chuyển sang trang thanh toán
                if (result.ErrorMessage == "Payment required")
                {
                    return RedirectToAction("Checkout", "Payment", new { courseId = courseId });
                }

                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Details", "Course", new { id = courseId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var result = await _enrollmentService.GetStudentEnrollmentsAsync();
            return View(result.Result);
        }
    }


}
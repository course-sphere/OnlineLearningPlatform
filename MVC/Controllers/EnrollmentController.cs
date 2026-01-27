using Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
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
            var result = await _enrollmentService.EnrollStudentDirectlyAsync(courseId);

            if (result.IsSuccess)
            {
                // Chuyển hướng sang trang Learn/Index
                return RedirectToAction("Index", "Learn", new { id = courseId });
            }

            TempData["Error"] = result.ErrorMessage;
            return RedirectToAction("Details", "Course", new { id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var result = await _enrollmentService.GetStudentEnrollmentsAsync();
            return View(result.Result);
        }
    }
}
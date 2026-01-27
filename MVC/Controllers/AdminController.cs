using Application.IServices;
using Domain.Entities;
using Domain.Requests.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICourseService _courseService;

        public AdminController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        // https://localhost:7276/Admin
        public IActionResult Index()
        {
            return View("Dashboard");
        }

        // https://localhost:7276/Admin/Users
        public IActionResult Users()
        {
            return View();
        }

        // https://localhost:7276/Admin/Payments
        public IActionResult Payments()
        {
            return View();
        }

        // https://localhost:7276/Admin/Reports
        public IActionResult Reports()
        {
            return View();
        }

        // https://localhost:7276/Admin/CourseStatistics
        public IActionResult CourseStatistics()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PendingCourses()
        {
            var result = await _courseService.GetAllCourseForAdminAsync(CourseStatus.PendingApproval);
            return View(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid courseId)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = true,
                RejectReason = null
            };

            var result = await _courseService.ApproveCourseAsync(request);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid courseId, string rejectReason)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = false,
                RejectReason = rejectReason
            };

            var result = await _courseService.ApproveCourseAsync(request);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> PreviewCourse(Guid id)
        {
            var result = await _courseService.GetCourseLearningDetailAsync(id);
            if (!result.IsSuccess) return RedirectToAction("Index");

            return View(result.Result);
        }
    }
}
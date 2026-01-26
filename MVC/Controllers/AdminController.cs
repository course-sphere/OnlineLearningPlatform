using Application.IServices;
using Domain.Entities;
using Domain.Requests.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    //[Authorize(Roles = "Admin")] // Chỉ Admin mới được vào
    public class AdminController : Controller
    {
        private readonly ICourseService _courseService;

        public AdminController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // 1. Danh sách khóa học chờ duyệt
        [HttpGet]
        public async Task<IActionResult> PendingCourses()
        {
            // Lấy các khóa học có status = PendingApproval (Enum value thường là 1 hoặc tùy định nghĩa)
            // Giả sử PendingApproval là trạng thái chờ duyệt
            var result = await _courseService.GetAllCourseForAdminAsync(CourseStatus.PendingApproval);
            return View(result.Result);
        }

        // 2. Duyệt khóa học
        [HttpPost]
        public async Task<IActionResult> Approve(Guid courseId)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = true // True = Approved
            };

            var result = await _courseService.ApproveCourseAsync(request);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Course approved successfully!";
            }
            else
            {
                TempData["Error"] = result.ErrorMessage;
            }
            return RedirectToAction("PendingCourses");
        }

        // 3. Từ chối khóa học (Kèm lý do)
        [HttpPost]
        public async Task<IActionResult> Reject(Guid courseId, string rejectReason)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = false, // False = Rejected
                RejectReason = rejectReason
            };

            var result = await _courseService.ApproveCourseAsync(request);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Course rejected.";
            }
            else
            {
                TempData["Error"] = result.ErrorMessage;
            }
            return RedirectToAction("PendingCourses");
        }
    }
}
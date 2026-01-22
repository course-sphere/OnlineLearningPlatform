using Application.IServices;
using Domain.Requests.Course;
using Domain.Responses.Course;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            // Giả sử service của bạn có hàm lấy chi tiết kèm theo Lessons
            var result = await _service.GetCourseDetailAsync(id);

            if (!result.IsSuccess)
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("Index");
            }

            return View(result.Result); // Trả về CourseDetailResponse
        }

        [Authorize(Roles= "Instructor,Admin")]
        [HttpGet]
        public IActionResult CreateNewCourse()
        {
            return View();
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewCourse(CreateNewCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _service.CreateNewCourseAsync(request);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(request);
            }
            
            var courseId = Guid.Parse(result.Result.ToString());
            TempData["Success"] = "Course created successfully! Now create lessons ^^";
            return RedirectToAction("Lesson", "CreateNewLesson", new { courseId = courseId});
        }

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllCourseForMemberAsync();

            if (result.IsSuccess)
            {
                // Trích xuất dữ liệu từ ApiResponse (giả định Data chứa List<CourseResponse>)
                var courses = result.Result as List<CourseResponse> ?? new List<CourseResponse>();
                return View(courses);
            }

            TempData["Error"] = result.ErrorMessage;
            return View(new List<CourseResponse>());
        }
    }
}

using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _courseService.GetCoursesByStatusAsync(CourseStatus.Published);
            return View(result.Result);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // Gọi hàm MỚI dành riêng cho Student (Isolation Strategy)
            var result = await _courseService.GetCourseDetailForStudentAsync(id);

            if (!result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            // Trả về View với DTO mới (StudentCourseDetailResponse)
            return View(result.Result);
        }
    }
}
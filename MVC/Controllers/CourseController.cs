using Application.IServices;
using Domain.Entities; // <-- Quan trọng: Để dùng được CourseStatus
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

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            if (!result.IsSuccess) return NotFound();

            return View(result.Result);
        }
    }
}
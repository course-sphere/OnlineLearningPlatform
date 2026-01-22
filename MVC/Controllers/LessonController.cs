using Application.IServices;
using Domain.Requests.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonService _service;

        public LessonController(ILessonService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpGet]
        [Route("CreateNewLesson/Lesson")]
        public IActionResult CreateNewLesson(Guid courseId)
        {
            var request = new CreateNewLessonRequest
            {
                CourseId = courseId
            };
            return View(request);
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewLesson(CreateNewLessonRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _service.CreateNewLesson(request);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(request);
            }
            TempData["Success"] = "Course created successfully!";
            return RedirectToAction("Index");
        }
    }
}

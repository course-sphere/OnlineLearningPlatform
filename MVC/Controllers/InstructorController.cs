using Application.IServices;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Requests.Lesson;
using Domain.Requests.LessonResource;
using Domain.Requests.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize(Roles = "Instructor")] // Chỉ giảng viên mới được vào
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IModuleService _moduleService;
        private readonly ILessonService _lessonService;
        private readonly ILessonResourceService _resourceService;

        public InstructorController(
            ICourseService courseService,
            IModuleService moduleService,
            ILessonService lessonService,
            ILessonResourceService resourceService)
        {
            _courseService = courseService;
            _moduleService = moduleService;
            _lessonService = lessonService;
            _resourceService = resourceService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _courseService.GetCoursesByInstructorAsync();
            return View(result.Result);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewCourseRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _courseService.CreateNewCourseAsync(request);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Course draft created! Let's build the curriculum.";
                
                return RedirectToAction("Edit", new { id = result.Result });
            }

            TempData["Error"] = result.ErrorMessage;
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var courseResult = await _courseService.GetCourseDetailAsync(id);
            if (!courseResult.IsSuccess) return NotFound();

            var modulesResult = await _moduleService.GetModulesByCourseAsync(id);

            ViewBag.Modules = modulesResult.Result; // Truyền tạm qua ViewBag hoặc tạo ViewModel riêng
            return View(courseResult.Result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] CreateNewModuleForCourseRequest request)
        {
            var result = await _moduleService.CreateNewModuleForCourseAsync(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] CreateNewLessonForModuleRequest request)
        {
            var result = await _lessonService.CreateNewLessonForModuleAsync(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadResource([FromForm] CreateLessonResourceRequest request)
        {
            var result = await _resourceService.CreateLessonResourceAsync(request);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonResources(Guid lessonId)
        {
            var result = await _resourceService.GetResourcesByLessonAsync(lessonId);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForReview(Guid id)
        {
            var result = await _courseService.SubmitCourseForReviewAsync(id);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Course submitted successfully! Admin will review it soon.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Edit", new { id = id });
            }
        }
    }
}
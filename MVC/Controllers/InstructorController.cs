using Application;
using Application.IServices;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Requests.Lesson;
using Domain.Requests.LessonResource;
using Domain.Requests.Module;
using Domain.Requests.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IModuleService _moduleService;
        private readonly ILessonService _lessonService;
        private readonly ILessonResourceService _resourceService;
        private readonly IQuestionService _questionService;
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(
            ICourseService courseService,
            IModuleService moduleService,
            ILessonService lessonService,
            ILessonResourceService resourceService,
            IQuestionService questionService,
            IUnitOfWork unitOfWork)
        {
            _courseService = courseService;
            _moduleService = moduleService;
            _lessonService = lessonService;
            _resourceService = resourceService;
            _questionService = questionService;
            _unitOfWork = unitOfWork;
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

            ViewBag.Modules = modulesResult.Result;
            return View(courseResult.Result);
        }

        [HttpGet]
        public async Task<IActionResult> EditQuiz(Guid id)
        {
            var gradedItem = await _unitOfWork.GradedItems.GetAsync(g => g.GradedItemId == id);
            if (gradedItem == null) return NotFound("Quiz/Assignment not found");

            var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == gradedItem.LessonId);
            var module = await _unitOfWork.Modules.GetAsync(m => m.ModuleId == lesson.ModuleId);
            var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == module.CourseId);

            ViewBag.CourseId = course.CourseId;
            ViewBag.CourseTitle = course.Title;
            ViewBag.LessonTitle = lesson.Title;

            var questions = await _unitOfWork.Questions.GetAllAsync(q => q.GradedItemId == id && !q.IsDeleted);

            foreach (var q in questions)
            {
                var answers = await _unitOfWork.AnswerOptions.GetAllAsync(a => a.QuestionId == q.QuestionId && !a.IsDeleted);
                q.AnswerOptions = answers.OrderBy(a => a.OrderIndex).ToList();
            }

            gradedItem.Questions = questions.OrderBy(q => q.OrderIndex).ToList();

            return View(gradedItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var result = await _questionService.CreateQuestionAsync(request);
            return Json(result);
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
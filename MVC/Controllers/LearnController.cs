using Application;
using Application.IServices;
using Domain.Entities;
using Domain.Responses.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    [Authorize]
    public class LearnController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _claimService;
        private readonly IUserLessonProgressService _progressService;

        public LearnController(IUnitOfWork unitOfWork, IClaimService claimService, IUserLessonProgressService progressService)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimService;
            _progressService = progressService;
        }

        // GET: /Learn/Index/{courseId}
        public async Task<IActionResult> Index(Guid id)
        {
            var userId = _claimService.GetUserClaim().UserId;

            // 1. Get Course Info
            var course = await _unitOfWork.Courses.GetQueryable()
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return NotFound();

            // 2. Get User Progress
            var userProgress = await _unitOfWork.UserLessonProgress.GetQueryable()
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();
            var completedSet = new HashSet<Guid>(userProgress);

            // 3. Get Modules (Fix: Dùng 'Index' thay vì 'OrderIndex')
            var modules = await _unitOfWork.Modules.GetQueryable()
                .Where(m => m.CourseId == id)
                .OrderBy(m => m.Index)
                .ToListAsync();

            // 4. MAP DATA
            var model = new CourseStudentLearningResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Instructor = "CourseSphere Instructor", // Entity chưa map User nên hardcode
                Description = course.Description ?? "Welcome to the course!",
                Level = course.Level.ToString(),
                Modules = new List<ModuleStudentResponse>()
            };

            foreach (var module in modules)
            {
                var moduleDto = new ModuleStudentResponse
                {
                    Id = module.ModuleId,
                    Title = module.Name, // Fix: Entity dùng 'Name'
                    IsFinal = false,
                    Lessons = new List<LessonStudentResponse>()
                };

                // Lấy Lessons + Resources + Quiz
                var lessons = await _unitOfWork.Lessons.GetQueryable()
                    .Where(l => l.ModuleId == module.ModuleId)
                    .Include(l => l.LessonResources)
                    .Include(l => l.GradedItems).ThenInclude(g => g.Questions).ThenInclude(q => q.AnswerOptions)
                    .OrderBy(l => l.OrderIndex)
                    .ToListAsync();

                foreach (var lesson in lessons)
                {
                    // Logic: Lấy Video đầu tiên trong Resource làm bài giảng chính
                    var mainVideo = lesson.LessonResources?.FirstOrDefault(r => r.ResourceType == ResourceType.Video);

                    var lessonDto = new LessonStudentResponse
                    {
                        Id = lesson.LessonId,
                        Title = lesson.Title,
                        // Fix: Entity Lesson không có Description, dùng Content đỡ hoặc để trống
                        Description = "",
                        Content = lesson.Content,

                        // Fix: Lấy URL từ Resource vì Lesson không có VideoUrl
                        VideoUrl = mainVideo?.ResourceUrl,

                        // Fix: Dùng EstimatedMinutes và ép kiểu về string
                        Duration = lesson.EstimatedMinutes + "m",

                        Completed = completedSet.Contains(lesson.LessonId),
                        Type = MapType(lesson.Type),
                        TypeLabel = lesson.Type.ToString(),

                        // Fix: Dùng ResourceUrl thay vì Url
                        Resources = lesson.LessonResources?
                            .Where(r => r.ResourceType != ResourceType.Video)
                            .Select(r => new ResourceStudentResponse
                            {
                                Id = r.LessonResourceId,
                                Title = r.Title,
                                Url = r.ResourceUrl, // Fix tên trường
                                Type = "download"
                            }).ToList() ?? new List<ResourceStudentResponse>()
                    };

                    // Check Quiz (Lấy GradedItem đầu tiên)
                    var quizItem = lesson.GradedItems?.FirstOrDefault();
                    if (quizItem != null)
                    {
                        lessonDto.Type = "quiz"; // UI cần type là quiz để hiện giao diện thi
                        lessonDto.Quiz = new QuizStudentResponse
                        {
                            Id = quizItem.GradedItemId,
                            Title = "Quiz: " + lesson.Title,
                            Kind = "multiple-choice",
                            Questions = quizItem.Questions?.Select(q => new QuestionStudentResponse
                            {
                                Id = q.QuestionId,
                                Text = q.Content,
                                Points = (int)q.Points, // Fix: Ép kiểu decimal -> int
                                Options = q.AnswerOptions?.Select(o => new AnswerOptionDTO
                                {
                                    Id = o.AnswerOptionId,
                                    Text = o.Text
                                }).ToList() ?? new List<AnswerOptionDTO>()
                            }).ToList() ?? new List<QuestionStudentResponse>()
                        };
                    }
                    moduleDto.Lessons.Add(lessonDto);
                }
                model.Modules.Add(moduleDto);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLesson(Guid id)
        {
            var result = await _progressService.MarkLessonCompletedAsync(id);
            if (result.IsSuccess) return Ok();
            return BadRequest(result.ErrorMessage);
        }

        // Helper map Enum sang string cho UI
        private string MapType(LessonType type)
        {
            return type switch
            {
                LessonType.Video => "video",
                LessonType.Reading => "reading",
                // Map cả Assignment thành Quiz để hiện giao diện làm bài
                LessonType.PracticeAssignment => "quiz",
                LessonType.GradedAssignment => "quiz",
                _ => "video"
            };
        }
    }
}
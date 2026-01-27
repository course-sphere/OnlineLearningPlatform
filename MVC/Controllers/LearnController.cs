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

        public async Task<IActionResult> Index(Guid id)
        {
            var userId = _claimService.GetUserClaim().UserId;
            var course = await _unitOfWork.Courses.GetQueryable().FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            var userProgress = await _unitOfWork.UserLessonProgress.GetQueryable()
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId).ToListAsync();
            var completedSet = new HashSet<Guid>(userProgress);

            var modules = await _unitOfWork.Modules.GetQueryable()
                .Where(m => m.CourseId == id).OrderBy(m => m.Index).ToListAsync();

            var model = new CourseStudentLearningResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Instructor = "CourseSphere Instructor",
                Description = course.Description ?? "",
                Level = course.Level.ToString(),
                Modules = new List<ModuleStudentResponse>()
            };

            foreach (var module in modules)
            {
                var moduleDto = new ModuleStudentResponse
                {
                    Id = module.ModuleId,
                    Title = module.Name,
                    IsFinal = false,
                    Lessons = new List<LessonStudentResponse>()
                };
                var lessons = await _unitOfWork.Lessons.GetQueryable()
                    .Where(l => l.ModuleId == module.ModuleId)
                    .Include(l => l.LessonResources)
                    .Include(l => l.GradedItems).ThenInclude(g => g.Questions).ThenInclude(q => q.AnswerOptions)
                    .OrderBy(l => l.OrderIndex).ToListAsync();

                foreach (var lesson in lessons)
                {
                    var mainVideo = lesson.LessonResources?.FirstOrDefault(r => r.ResourceType == ResourceType.Video);
                    var lessonDto = new LessonStudentResponse
                    {
                        Id = lesson.LessonId,
                        Title = lesson.Title,
                        Description = "",
                        Content = lesson.Content,
                        VideoUrl = mainVideo?.ResourceUrl,
                        Duration = lesson.EstimatedMinutes + "m",
                        Completed = completedSet.Contains(lesson.LessonId),
                        Type = MapType(lesson.Type),
                        TypeLabel = lesson.Type.ToString(),
                        Resources = lesson.LessonResources?.Where(r => r.ResourceType != ResourceType.Video)
                            .Select(r => new ResourceStudentResponse { Id = r.LessonResourceId, Title = r.Title, Url = r.ResourceUrl, Type = "download" }).ToList() ?? new List<ResourceStudentResponse>()
                    };
                    var quizItem = lesson.GradedItems?.FirstOrDefault();
                    if (quizItem != null)
                    {
                        lessonDto.Type = "quiz";
                        lessonDto.Quiz = new QuizStudentResponse
                        {
                            Id = quizItem.GradedItemId,
                            Title = "Quiz: " + lesson.Title,
                            Kind = "multiple-choice",
                            PassingScore = 50,
                            Questions = quizItem.Questions?.Select(q => new QuestionStudentResponse
                            {
                                Id = q.QuestionId,
                                Text = q.Content,
                                Points = (int)q.Points,
                                Options = q.AnswerOptions?.Select(o => new AnswerOptionDTO { Id = o.AnswerOptionId, Text = o.Text }).ToList() ?? new List<AnswerOptionDTO>()
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
            if (result.IsSuccess)
            {
                var userId = _claimService.GetUserClaim().UserId;
                var lesson = await _unitOfWork.Lessons.GetQueryable().Include(l => l.Module).FirstOrDefaultAsync(l => l.LessonId == id);
                if (lesson != null)
                {
                    var courseId = lesson.Module.CourseId;
                    var totalLessons = await _unitOfWork.Lessons.GetQueryable().CountAsync(l => l.Module.CourseId == courseId);
                    var completedCount = await _unitOfWork.UserLessonProgress.GetQueryable()
                        .CountAsync(p => p.UserId == userId && p.IsCompleted && p.Lesson.Module.CourseId == courseId);

                    var newPercent = totalLessons == 0 ? 0 : (int)((double)completedCount / totalLessons * 100);
                    return Ok(new { percent = newPercent, completed = true });
                }
            }
            return Ok(new { percent = 0, completed = true });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmissionModel model)
        {
            int correctCount = 0;
            int totalQuestions = model.Answers.Count;

            foreach (var ans in model.Answers)
            {
                var option = await _unitOfWork.AnswerOptions.GetQueryable()
                    .FirstOrDefaultAsync(o => o.AnswerOptionId == ans.Value);

                if (option != null && option.IsCorrect) correctCount++;
            }

            double score = totalQuestions == 0 ? 0 : ((double)correctCount / totalQuestions) * 100;
            bool isPassed = score >= 50;

            if (isPassed)
            {
                return Ok(new { passed = true, redirectUrl = "/Learn/QuizResultPass", score = score });
            }
            else
            {
                return Ok(new { passed = false, redirectUrl = "/Learn/QuizResultFail", score = score });
            }
        }

        // 👇 ĐÃ GỘP: CHỈ GIỮ LẠI PHIÊN BẢN CÓ THAM SỐ
        [HttpGet]
        public IActionResult QuizResultPass(double score, Guid courseId)
        {
            ViewBag.Score = score;
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpGet]
        public IActionResult QuizResultFail(double score, Guid courseId)
        {
            ViewBag.Score = score;
            ViewBag.CourseId = courseId;
            return View();
        }

        private string MapType(LessonType type) => type switch
        {
            LessonType.Video => "video",
            LessonType.Reading => "reading",
            LessonType.PracticeAssignment => "quiz",
            LessonType.GradedAssignment => "quiz",
            _ => "video"
        };
    }

    public class QuizSubmissionModel
    {
        public Guid QuizId { get; set; }
        public Dictionary<Guid, Guid> Answers { get; set; }
    }
}
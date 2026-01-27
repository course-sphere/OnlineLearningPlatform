using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Responses;
using Domain.Responses.Course;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IClaimService _service;
        private readonly IEmailService _emailService;

        public CourseService(IMapper mapper, IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService, IClaimService service, IEmailService emailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _firebaseStorageService = firebaseStorageService;
            _service = service;
            _emailService = emailService;
        }

        public async Task<ApiResponse> CreateNewCourseAsync(CreateNewCourseRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();
                var course = _mapper.Map<Course>(request);
                course.CreatedBy = claim.UserId;
                if (request.ImageFile != null)
                {
                    var imageUrl = await _firebaseStorageService.UploadCourseImage(request.Title, request.ImageFile);
                    course.Image = imageUrl;
                }
                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(course.CourseId);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllCourseAsync()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var courses = await _unitOfWork.Courses.GetAllAsync(c => c.Status == CourseStatus.Published);
                var courseResponses = _mapper.Map<List<CourseResponse>>(courses);
                return response.SetOk(courseResponses);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> GetCourseDetailAsync(Guid courseId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null)
                {
                    return response.SetNotFound("Course not found");
                }
                var courseResponse = _mapper.Map<CourseResponse>(course);
                return response.SetOk(courseResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateCourseAsync(UpdateCourseRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == request.CourseId);
                if (course == null)
                {
                    return response.SetNotFound("Course not found");
                }
                var userId = _service.GetUserClaim().UserId;
                var updatedCourse = _mapper.Map(request, course);
                updatedCourse.UpdatedAt = DateTime.UtcNow;
                updatedCourse.UpdatedBy = userId;

                _unitOfWork.Courses.Update(updatedCourse);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Course updated successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteCourseAsync(Guid courseId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null)
                {
                    return response.SetNotFound("Course not found");
                }
                course.IsDeleted = true;
                _unitOfWork.Courses.Update(course);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Course deleted successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllCourseForAdminAsync(CourseStatus status)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var courses = await _unitOfWork.Courses.GetAllAsync(c => c.Status == status);
                if (courses == null) return null;

                var result = new List<GetAllCourseForAdminResponse>();

                foreach (var course in courses)
                {
                    var modules = await _unitOfWork.Modules.GetAllAsync(m => m.CourseId == course.CourseId);
                    var moduleIds = modules.Select(m => m.ModuleId).ToList();

                    var lessons = await _unitOfWork.Lessons.GetAllAsync(l => moduleIds.Contains(l.ModuleId));
                    var lessonIds = lessons.Select(l => l.LessonId).ToList();

                    var courseMapping = _mapper.Map<GetAllCourseForAdminResponse>(course);

                    courseMapping.ModuleCount = modules.Count;
                    courseMapping.LessonCount = lessons.Count;
                    courseMapping.VideoCount = lessons.Count(l => l.Type == LessonType.Video);
                    courseMapping.ReadingCount = lessons.Count(l => l.Type == LessonType.Reading);

                    result.Add(courseMapping);
                }
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> GetCoursesByInstructorAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var claim = _service.GetUserClaim();
                var courses = await _unitOfWork.Courses
                    .GetAllAsync(c => c.CreatedBy == claim.UserId && !c.IsDeleted);

                var result = _mapper.Map<List<CourseResponse>>(courses);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetEnrolledCoursesForStudentAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var studentId = _service.GetUserClaim().UserId;
                var enrollments = await _unitOfWork.Enrollments
                    .GetAllAsync(e => e.UserId == studentId);

                var courseIds = enrollments
                    .Select(e => e.CourseId)
                    .Distinct()
                    .ToList();

                var courses = await _unitOfWork.Courses
                    .GetAllAsync(c => courseIds.Contains(c.CourseId));

                var result = _mapper.Map<List<CourseResponse>>(courses);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> ApproveCourseAsync(ApproveCourseRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == request.CourseId && !c.IsDeleted);
                if (course == null) return response.SetNotFound("Course not found");

                var instructor = await _unitOfWork.Users.GetAsync(u => u.UserId == course.CreatedBy);

                if (!request.Status)
                {
                    if (string.IsNullOrEmpty(request.RejectReason))
                        return response.SetBadRequest("Reject reason is required");

                    course.Status = CourseStatus.Rejected;
                    course.RejectReason = request.RejectReason;
                    course.UpdatedAt = DateTime.UtcNow;
                    course.UpdatedBy = _service.GetUserClaim().UserId;

                    _unitOfWork.Courses.Update(course);
                    await _unitOfWork.SaveChangeAsync();

                    if (instructor != null)
                    {
                        await _emailService.SendRejectCourseEmail(instructor.FullName, instructor.Email, request.RejectReason, course.Title);
                    }

                    return response.SetOk("Course rejected & email sent");
                }
                else
                {
                    course.Status = CourseStatus.Published;
                    course.RejectReason = string.Empty; 
                    course.UpdatedAt = DateTime.UtcNow;
                    course.UpdatedBy = _service.GetUserClaim().UserId;

                    _unitOfWork.Courses.Update(course);
                    await _unitOfWork.SaveChangeAsync();

                    
                    if (instructor != null)
                    {
                        await _emailService.SendApproveCourseEmail(
                            receiverName: instructor.FullName,
                            receiverEmail: instructor.Email,
                            courseTitle: course.Title
                        );
                    }

                    return response.SetOk("Course approved & email sent");
                }
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> SubmitCourseForReviewAsync(Guid courseId)
        {
            var response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null) return response.SetNotFound("Course not found");
                course.Status = CourseStatus.PendingApproval;

                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Course submitted for review.");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetCoursesByStatusAsync(CourseStatus status)
        {
            var response = new ApiResponse();
            try
            {
                var courses = await _unitOfWork.Courses.GetAllAsync(c => c.Status == status && !c.IsDeleted);

                var courseResponses = _mapper.Map<List<CourseResponse>>(courses);


                return response.SetOk(courseResponses);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetCourseByIdAsync(Guid courseId)
        {
            var response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null) return response.SetNotFound("Course not found");

                var courseResponse = _mapper.Map<CourseResponse>(course);
                return response.SetOk(courseResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetCourseLearningDetailAsync(Guid courseId)
        {
            try
            {
                // 1. Get Course & Modules & Lessons
                // Lưu ý: Cần Include rất sâu. Nếu dùng EF Core thuần thì chuỗi include rất dài.
                // Tốt nhất là load Course -> Load Modules theo CourseId -> Load Lessons theo ModuleId

                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null) return new ApiResponse().SetNotFound("Course not found");

                var instructor = await _unitOfWork.Users.GetAsync(u => u.UserId == course.CreatedBy);

                var response = new CourseLearningResponse
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Instructor = instructor?.FullName ?? "Instructor",
                    Description = course.Description,
                    Level = course.Level.ToString(),
                    Rating = 5.0, // Fake tạm
                    Students = 120, // Fake tạm
                    Duration = "10h 30m"
                };

                // 2. Get Modules
                var modules = await _unitOfWork.Modules.GetAllAsync(m => m.CourseId == courseId);
                modules = modules.OrderBy(m => m.Index).ToList();

                foreach (var mod in modules)
                {
                    var modRes = new ModuleLearningResponse
                    {
                        Id = mod.ModuleId,
                        Title = mod.Name,
                        Duration = "1h" // Tính tổng sau
                    };

                    // 3. Get Lessons
                    var lessons = await _unitOfWork.Lessons.GetAllAsync(l => l.ModuleId == mod.ModuleId && !l.IsDeleted);
                    lessons = lessons.OrderBy(l => l.OrderIndex).ToList();

                    foreach (var lesson in lessons)
                    {
                        // Map Lesson Type sang chuỗi UI cần
                        string type = "reading";
                        if (lesson.Type == Domain.Entities.LessonType.Video) type = "video";
                        if (lesson.Type == Domain.Entities.LessonType.GradedAssignment || lesson.Type == Domain.Entities.LessonType.PracticeAssignment) type = "quiz";

                        var lessonRes = new LessonLearningResponse
                        {
                            Id = lesson.LessonId,
                            Title = lesson.Title,
                            Duration = lesson.EstimatedMinutes + " min",
                            Type = type,
                            TypeLabel = lesson.Type.ToString(),
                            Description = lesson.Content ?? "",
                            Cover = course.Image // Dùng tạm ảnh course
                        };

                        // 4. Nếu là Video -> Lấy Resource Video
                        var resources = await _unitOfWork.LessonResources.GetAllAsync(r => r.LessonId == lesson.LessonId);
                        if (type == "video")
                        {
                            var vid = resources.FirstOrDefault(r => r.ResourceType == Domain.Entities.ResourceType.Video);
                            lessonRes.VideoUrl = vid?.ResourceUrl;
                        }

                        // 5. Nếu là Quiz -> Lấy GradedItem & Question
                        if (type == "quiz")
                        {
                            // Load Quiz Data (Vì Generic Repo ko hỗ trợ Include sâu, ta load thủ công)
                            var gradedItems = await _unitOfWork.GradedItems.GetAllAsync(g => g.LessonId == lesson.LessonId);
                            var quiz = gradedItems.FirstOrDefault();

                            if (quiz != null)
                            {
                                var questions = await _unitOfWork.Questions.GetAllAsync(q => q.GradedItemId == quiz.GradedItemId && !q.IsDeleted);

                                lessonRes.Quiz = new QuizLearningResponse
                                {
                                    Id = quiz.GradedItemId,
                                    Title = lesson.Title,
                                    Kind = "multiple-choice", // UI chỉ support cái này mượt nhất hiện tại
                                    PassingScore = 50,
                                    Questions = new List<QuestionLearningResponse>()
                                };

                                foreach (var q in questions)
                                {
                                    var options = await _unitOfWork.AnswerOptions.GetAllAsync(a => a.QuestionId == q.QuestionId);
                                    lessonRes.Quiz.Questions.Add(new QuestionLearningResponse
                                    {
                                        Id = q.QuestionId,
                                        Text = q.Content,
                                        Points = (int)q.Points,
                                        Options = options.Select(o => o.Text).ToList() // Chỉ lấy Text
                                    });
                                }
                            }
                        }

                        modRes.Lessons.Add(lessonRes);
                    }
                    response.Modules.Add(modRes);
                }

                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(ex.Message);
            }
        }

    }
}

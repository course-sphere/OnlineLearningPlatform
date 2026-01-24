using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Responses;
using Domain.Responses.Course;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IClaimService _service;

        public CourseService(IMapper mapper, IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService, IClaimService service)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _firebaseStorageService = firebaseStorageService;
            _service = service;
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
                var course = await _unitOfWork.Courses
                    .GetAsync(c => c.CourseId == request.CourseId && !c.IsDeleted);

                if (course == null)
                    return response.SetNotFound("Course not found");
                if (request.Status == false && string.IsNullOrEmpty(request.RejectReason))
                    return response.SetBadRequest("Reject reason is required when rejecting a course");
                if (request.Status == false)
                {
                    course.Status = CourseStatus.Rejected;
                    course.RejectReason = request.RejectReason;
                    course.UpdatedAt = DateTime.UtcNow;
                    course.UpdatedBy = _service.GetUserClaim().UserId;
                    _unitOfWork.Courses.Update(course);
                    await _unitOfWork.SaveChangeAsync();
                    return response.SetOk("Course rejected successfully");
                }
                else 
                {
                    course.Status = CourseStatus.Published;
                    course.UpdatedAt = DateTime.UtcNow;
                    course.RejectReason = "";
                    course.UpdatedBy = _service.GetUserClaim().UserId;
                    _unitOfWork.Courses.Update(course);
                    await _unitOfWork.SaveChangeAsync();
                    return response.SetOk("Course approved successfully");
                }
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

    }
}

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

                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(course.CourseId);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllCourseForMemberAsync()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var courses = await _unitOfWork.Courses.GetAllAsync();
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
    }
}

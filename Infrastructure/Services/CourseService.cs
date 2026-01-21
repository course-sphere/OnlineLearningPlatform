using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Responses;

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

        public async Task<ApiResponse> CreateNewCourse(CreateNewCourseRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();
                var course = _mapper.Map<Course>(request);
                course.CreatedBy = claim.UserId;

                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk($"Course {course.Title} has been created successfully.");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}

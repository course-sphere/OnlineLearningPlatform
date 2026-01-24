using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Lesson;
using Domain.Responses;

namespace Infrastructure.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IClaimService _service;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewLessonForModuleAsync(CreateNewLessonForModuleRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();

                var module = await _unitOfWork.Modules.GetAsync(m => m.ModuleId == request.ModuleId);
                if (module == null)
                {
                    return response.SetNotFound(message: "Module not found or may have been automatically deleted due to inactivity!!! Please check your course");
                }
                var lesson = _mapper.Map<Lesson>(request);
                lesson.CreatedBy = claim.UserId;
                await _unitOfWork.Lessons.AddAsync(lesson);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk($"Lesson {lesson.OrderIndex} {lesson.Title} has been created successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}

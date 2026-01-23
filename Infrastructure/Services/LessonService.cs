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

        public async Task<ApiResponse> CreateNewLesson(CreateNewLessonRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();

                var lesson = _mapper.Map<Lesson>(request);
                lesson.CreatedBy = claim.UserId;
                await _unitOfWork.Lessons.AddAsync(lesson);
                await _unitOfWork.SaveChangeAsync();
                if (!request.ParentLessonId.HasValue)
                {
                    return response.SetOk($"Lesson {lesson.Title} has been created successfully."
                    );
                }
                var parentLesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == request.ParentLessonId.Value);

                return response.SetOk($"Lesson {lesson.Title} has been created successfully under {parentLesson.Title}");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}

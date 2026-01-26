using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Lesson;
using Domain.Responses;
using Domain.Responses.Lesson;

namespace Infrastructure.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _service;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                var result = _mapper.Map<LessonResponse>(lesson);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateLessonAsync(Guid lessonId, UpdateLessonRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == lessonId);
                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                _mapper.Map(request, lesson);
                lesson.UpdatedBy = _service.GetUserClaim().UserId;

                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Lesson updated successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteLessonAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == lessonId);
                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                _unitOfWork.Lessons.RemoveIdAsync(lesson.LessonId);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Lesson deleted successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> GetLessonsByModuleAsync(Guid moduleId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lessons = await _unitOfWork.Lessons.GetAllAsync(
                    l => l.ModuleId == moduleId
                );

                var result = new
                {
                    Total = lessons.Count(),
                    VideoCount = lessons.Count(l => l.Type == LessonType.Video),
                    ReadingCount = lessons.Count(l => l.Type == LessonType.Reading),
                    PracticeCount = lessons.Count(l => l.Type == LessonType.PracticeAssignment),
                    GradedCount = lessons.Count(l => l.Type == LessonType.GradedAssignment),
                    Lessons = _mapper.Map<List<LessonResponse>>(lessons)
                };

                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetLessonDetailAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons.GetAsync(
                    l => l.LessonId == lessonId);

                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                var result = _mapper.Map<LessonDetailResponse>(lesson);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

    }
}

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
                if (module == null) return response.SetNotFound("Module not found!");

                var existingLessons = await _unitOfWork.Lessons.GetAllAsync(l => l.ModuleId == request.ModuleId && !l.IsDeleted);
                int newOrderIndex = existingLessons.Any() ? existingLessons.Max(l => l.OrderIndex) + 1 : 1;

                var lesson = _mapper.Map<Lesson>(request);
                lesson.CreatedBy = claim.UserId;
                lesson.OrderIndex = newOrderIndex;

                lesson.GradedItems = new List<GradedItem>();

                await _unitOfWork.Lessons.AddAsync(lesson);

                if (lesson.Type == LessonType.GradedAssignment || lesson.Type == LessonType.PracticeAssignment)
                {
                    var gradedItem = new GradedItem
                    {
                        GradedItemId = Guid.NewGuid(), // Tạo ID luôn để dùng ngay
                        LessonId = lesson.LessonId,
                        Type = GradedItemType.Quiz,
                        MaxScore = 100,
                        IsAutoGraded = true,
                        CreatedBy = claim.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.GradedItems.AddAsync(gradedItem);

                    // ===> QUAN TRỌNG: Gán ngược lại vào lesson để Mapper thấy data ngay lập tức <===
                    lesson.GradedItems.Add(gradedItem);
                }

                await _unitOfWork.SaveChangeAsync();

                // Lúc này lesson.GradedItems đã có dữ liệu, Mapper sẽ hoạt động đúng
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

                await _unitOfWork.Lessons.RemoveIdAsync(lesson.LessonId);
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

                lessons = lessons.OrderBy(l => l.OrderIndex).ToList();

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
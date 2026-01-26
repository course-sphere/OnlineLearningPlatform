using Domain.Requests.Lesson;
using Domain.Responses;

namespace Application.IServices
{
    public interface ILessonService
    {
        Task<ApiResponse> CreateNewLessonForModuleAsync(CreateNewLessonForModuleRequest request);
        Task<ApiResponse> UpdateLessonAsync(Guid lessonId, UpdateLessonRequest request);
        Task<ApiResponse> DeleteLessonAsync(Guid lessonId);

        Task<ApiResponse> GetLessonDetailAsync(Guid lessonId);
        Task<ApiResponse> GetLessonsByModuleAsync(Guid moduleId);
    }
}

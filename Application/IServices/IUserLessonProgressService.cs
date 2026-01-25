using Domain.Responses;
using Domain.Requests.UserLessonProgress;   
namespace Application.IServices
{
    public interface IUserLessonProgressService
    {
        Task<ApiResponse> StartOrUpdateProgressAsync(UpdateUserLessonProgressRequest request);
        Task<ApiResponse> MarkLessonCompletedAsync(Guid lessonId);
        Task<ApiResponse> GetLessonProgressAsync(Guid lessonId);
        Task<ApiResponse> GetLessonProgressByUserAsync(Guid userId);
    }
}

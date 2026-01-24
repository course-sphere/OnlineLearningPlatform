using Domain.Responses;
using Domain.Requests.UserLessonProgress;   
namespace Application.IServices
{
    public interface IUserLessonProgressService
    {
        Task<ApiResponse> StartLessonAsync(Guid lessonId);
        Task<ApiResponse> UpdateProgressAsync(UpdateUserLessonProgressRequest request);
        Task<ApiResponse> CompleteLessonAsync(Guid lessonId);
        Task<ApiResponse> GetProgressByLessonAsync(Guid lessonId);
    }
}

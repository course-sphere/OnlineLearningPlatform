using Domain.Requests.Lesson;
using Domain.Responses;

namespace Application.IServices
{
    public interface ILessonService
    {
        Task<ApiResponse> CreateNewLesson(CreateNewLessonRequest request);
    }
}

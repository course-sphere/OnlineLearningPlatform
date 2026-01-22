using Domain.Requests.Course;
using Domain.Responses;

namespace Application.IServices
{
    public interface ICourseService
    {
        Task<ApiResponse> CreateNewCourseAsync(CreateNewCourseRequest request);
        Task<ApiResponse> GetAllCourseForMemberAsync();
        Task<ApiResponse> GetCourseDetailAsync(Guid courseId);
    }
}

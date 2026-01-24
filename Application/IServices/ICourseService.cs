using Domain.Entities;
using Domain.Requests.Course;
using Domain.Responses;

namespace Application.IServices
{
    public interface ICourseService
    {
        Task<ApiResponse> CreateNewCourseAsync(CreateNewCourseRequest request);
        Task<ApiResponse> GetAllCourseAsync();
        Task<ApiResponse> GetCourseDetailAsync(Guid courseId);
        Task<ApiResponse> GetAllCourseForAdminAsync(CourseStatus status);
    }
}

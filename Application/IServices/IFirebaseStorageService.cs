using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadCourseImage(string courseName, IFormFile? file);
        Task<string> UploadUserImage(string userName, IFormFile? file);
    }
}

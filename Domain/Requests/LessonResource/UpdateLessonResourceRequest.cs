using Microsoft.AspNetCore.Http;

namespace Domain.Requests.LessonResource
{
    public class UpdateLessonResourceRequest
    {
        public string? Title { get; set; }
        public IFormFile? File { get; set; }
    }
}

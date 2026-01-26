using Microsoft.AspNetCore.Http;

namespace Domain.Requests.LessonResource
{
    public class CreateLessonResourceRequest
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; }
        public IFormFile File { get; set; }
        public bool IsDownloadable { get; set; }
        public int OrderIndex { get; set; }
    }
}

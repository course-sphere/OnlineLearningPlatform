using Microsoft.AspNetCore.Http;

namespace Domain.Requests.Course
{
    public class CreateNewCourseRequest
    {
        public string Title { get ; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
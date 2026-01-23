namespace Domain.Requests.Module
{
    public class CreateNewModuleForCourseRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CourseId { get; set; }
    }
}

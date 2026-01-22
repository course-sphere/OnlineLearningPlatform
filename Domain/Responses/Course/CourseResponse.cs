namespace Domain.Responses.Course
{
    public class CourseResponse
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }   
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
    }
}

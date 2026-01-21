namespace Domain.Entities
{
    public class Course : Base
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool IsPublished { get; set; } = false;
        public decimal Price { get; set; }
        public List<Lesson>? Lessons { get; set; }
        public List<Assignment>? Assignments { get; set; } 
        public List<Quiz>? Quizzes { get; set; }
        public List<Enrollment>? Enrollments { get; set; }

    }
}

namespace Domain.Responses.Course
{
    public class CourseDetailResponse
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string Level { get; set; } = "Beginner";
        public string Category { get; set; } = "Development";
        public double Rating { get; set; }
        public int Students { get; set; }
        public string Duration { get; set; }
        public string? InstructorName { get; set; }
        public Guid InstructorId { get; set; }

        public List<CourseDetailModule> Modules { get; set; } = new List<CourseDetailModule>();
    }

    public class CourseDetailModule
    {
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public List<CourseDetailLesson> Lessons { get; set; } = new List<CourseDetailLesson>();
    }

    public class CourseDetailLesson
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
    }
}
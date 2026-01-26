namespace Domain.Entities
{
    public class Quiz : Base
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public TimeOnly? TimeLimit { get; set; }
        public decimal TotalPoint { get; set; }
        public Guid CourseId { get; set; }

        // Navigation
        public Course? Course { get; set; }
        public List<Question>? Questions { get; set; } = new();
        public List<QuizAttempt>? Attempts { get; set; } = new();
    }
}

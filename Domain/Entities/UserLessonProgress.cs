namespace Domain.Entities
{
    public class UserLessonProgress
    {
        public Guid LessonProgressId { get; set; }
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public int? LastWatchedSecond { get; set; }
        public int? CompletionPercent { get; set; }
        public User? User { get; set; }
        public Lesson? Lesson { get; set; }
    }
}

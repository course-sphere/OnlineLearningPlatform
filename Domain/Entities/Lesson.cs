namespace Domain.Entities
{
    public class Lesson : Base
    {
        public Guid LessonId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int EstimatedMinutes { get; set; }
        public bool IsGraded { get; set; }
        public int OrderIndex { get; set; }
        public LessonType Type { get; set; }
        public Module? Module { get; set; }
        public List<GradedItem>? GradedItems { get; set; }
        public List<UserLessonProgress>? UserLessonProgresses { get; set; }
        public List<LessonResource>? LessonResources { get; set; }
    }

    public enum LessonType
    {
        Video,
        Reading,
        PracticeAssignment,
        GradedAssignment    
    }
}

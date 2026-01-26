namespace Domain.Entities
{
    public class GradedItem : Base
    {
        public Guid GradedItemId { get; set; }
        public Guid LessonId { get; set; }
        public GradedItemType Type { get; set; }
        public int MaxScore { get; set; }
        public bool IsAutoGraded { get; set; }
        public Lesson? Lesson { get; set; }
        public List<GradedAttempt>? GradedAttempts { get; set; }
        public List<Question>? Questions { get; set; }
    }

    public enum GradedItemType
    {
        Quiz,
        Assignment,
    }
}

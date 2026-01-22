namespace Domain.Entities
{
    public class GradedAttempt : Base
    {
        public Guid GradedAttemptId { get; set; }
        public Guid UserId { get; set; }
        public Guid GradedItemId { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? GradedAt { get; set; }
        public decimal? Score { get; set; }
        public int AttemptNumber { get; set; }
        public GradedAttemptStatus Status { get; set; }
        public User? User { get; set; }
        public GradedItem? GradedItem { get; set; }
        public List<QuestionSubmission>? QuestionSubmissions { get; set; }
    }

    public enum GradedAttemptStatus
    {
        InProgress,
        Graded,
        Submitted
    }
}

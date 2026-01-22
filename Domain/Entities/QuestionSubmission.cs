namespace Domain.Entities
{
    public class QuestionSubmission : Base
    {
        public Guid QuestionSubmissionId { get; set; }
        public Guid GradedAttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public string? FileUrl { get; set; }
        public string? AnswerText { get; set; }
        public decimal? Score { get; set; } 
        public bool IsAutoGraded { get; set; }
        public GradedAttempt? GradedAttempt { get; set; }
        public Question? Question { get; set; }
        public List<SubmissionAnswerOption>? SelectedOptions { get; set; } = new();
    }
}

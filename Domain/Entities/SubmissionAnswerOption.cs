namespace Domain.Entities
{
    public class SubmissionAnswerOption
    {
        public Guid SubmissionAnswerOptionId { get; set; }
        public Guid QuestionSubmissionId { get; set; }
        public Guid AnswerOptionId { get; set; }
        public decimal Weight { get; set; }
        public bool IsCorrect { get; set; } = false;
        public QuestionSubmission? QuestionSubmission { get; set; }
        public AnswerOption? AnswerOption { get; set; }
    }
}

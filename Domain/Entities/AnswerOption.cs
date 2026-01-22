namespace Domain.Entities
{
    public class AnswerOption : Base
    {
        public Guid AnswerOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCorrect { get; set; }
        public decimal Weight { get; set; } = 1;
        public Question? Question { get; set; }
        public List<SubmissionAnswerOption>? SubmissionAnswerOptions { get; set; }
    }
}

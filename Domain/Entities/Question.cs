namespace Domain.Entities
{
    public class Question : Base
    {
        public Guid QuestionId { get; set; }
        public Guid QuizId { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public decimal Points { get; set; }
        // Navigation
        public Quiz? Quiz { get; set; }
        public List<AnswerOption>? AnswerOptions { get; set; } = new();
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer
    }
}

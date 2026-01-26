namespace Domain.Entities
{
    public class Question : Base
    {
        public Guid QuestionId { get; set; }
        public Guid GradedItemId { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public decimal Points { get; set; }
        public int OrderIndex { get; set; }
        public bool? IsRequired { get; set; }
        public string? Explanation { get; set; }
        // Navigation
        public GradedItem? GradedItem { get; set; }
        public List<AnswerOption>? AnswerOptions { get; set; } 
        public List<QuestionSubmission>? QuestionSubmissions { get; set; } 
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer
    }
}

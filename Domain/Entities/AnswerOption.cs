namespace Domain.Entities
{
    public class AnswerOption : Base
    {
        public Guid AnswerOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public OptionType Type { get; set; }
        public Question? Question { get; set; }
    }

    public enum OptionType
    {
        Correct,
        Incorrect
    }
}

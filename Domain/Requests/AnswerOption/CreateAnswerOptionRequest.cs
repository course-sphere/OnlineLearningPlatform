namespace Domain.Requests.AnswerOption
{
    public class CreateAnswerOptionRequest
    {
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public string? Explanation { get; set; }    
        public bool IsCorrect { get; set; }
        public decimal Weight { get; set; }
    }
}

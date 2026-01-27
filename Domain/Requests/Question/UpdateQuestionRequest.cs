using Domain.Entities;
using Domain.Requests.AnswerOption;

namespace Domain.Requests.Question
{
    public class UpdateQuestionRequest
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public decimal Points { get; set; }
        public string? Explanation { get; set; }
        public QuestionType Type { get; set; }

        public List<CreateAnswerOptionRequest>? AnswerOptions { get; set; }
    }
}
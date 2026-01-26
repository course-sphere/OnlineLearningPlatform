using Domain.Entities;
using Domain.Requests.AnswerOption;

namespace Domain.Requests.Question
{
    public class CreateQuestionRequest
    {
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public decimal Points { get; set; }
        public string Explanation { get; set; } 
        public int OrderIndex { get; set; }
        public List<CreateAnswerOptionRequest>? AnswerOptions { get; set; }
    }
}

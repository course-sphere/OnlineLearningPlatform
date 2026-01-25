using Domain.Entities;
using Domain.Requests.Question;

namespace Domain.Requests.GradedItem
{
    public class CreateGradedItemRequest
    {
        public Guid LessonId { get; set; }
        public GradedItemType Type { get; set; }
        public int MaxScore { get; set; }
        public List<CreateQuestionRequest>? Questions { get; set; }
    }
}

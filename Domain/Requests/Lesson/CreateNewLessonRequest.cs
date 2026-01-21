namespace Domain.Requests.Lesson
{
    public class CreateNewLessonRequest
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
        public Guid? ParentLessonId { get; set; }
    }
}

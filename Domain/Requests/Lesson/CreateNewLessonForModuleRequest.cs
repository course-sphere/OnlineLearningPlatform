namespace Domain.Requests.Lesson
{
    public class CreateNewLessonForModuleRequest
    {
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int OrderIndex { get; set; }
    }
}

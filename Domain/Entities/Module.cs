namespace Domain.Entities
{
    public class Module : Base
    {
        public Guid ModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Index { get; set; }
        public bool IsPublished { get; set; } = false;
        public Course? Course { get; set; }
        public List<Lesson>? Lessons { get; set; }
    }
}

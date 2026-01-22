namespace Domain.Entities
{
    public class Lesson : Base
    {
        public Guid LessonId { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int? Index { get; set; } // Thứ tự của bài học trong khóa học
        public Guid? ParentLessonId { get; set; } // Id của bài học cha nếu có
        public Course? Course { get; set; }  
        public Lesson? ParentLesson { get; set; }
        public List<Lesson>? SubLesson { get; set; }
    }
}

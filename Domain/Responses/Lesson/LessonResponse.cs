using System;
using Domain.Entities;

namespace Domain.Responses.Lesson
{
    public class LessonResponse
    {
        public Guid LessonId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int OrderIndex { get; set; }

        public LessonType Type { get; set; }
        public bool IsGraded { get; set; }
        public int EstimatedMinutes { get; set; }
    }
}
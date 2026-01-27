using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Responses.GradedItem;

namespace Domain.Responses.Lesson
{
    public class LessonResponse
    {
        public Guid LessonId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }

        public int OrderIndex { get; set; }

        public LessonType Type { get; set; }
        public bool IsGraded { get; set; }
        public int EstimatedMinutes { get; set; }
        public List<GradedItemResponse>? GradedItems { get; set; }
    }
}
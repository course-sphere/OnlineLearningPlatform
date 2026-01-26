using System;
using Domain.Entities; 

namespace Domain.Responses.LessonResource
{
    public class LessonResourceResponse
    {
        public Guid LessonResourceId { get; set; }
        public string Title { get; set; }
        public string ResourceUrl { get; set; }
        public ResourceType ResourceType { get; set; }
        public Guid LessonId { get; set; }
    }
}
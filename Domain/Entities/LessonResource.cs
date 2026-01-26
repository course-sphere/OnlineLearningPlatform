namespace Domain.Entities
{
    public class LessonResource : Base
    {
        public Guid LessonResourceId { get; set; }
        public Guid LessonId { get; set; }
        public string Title { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public int OrderIndex { get; set; }
        public long? DurationInSeconds { get; set; }
        public bool IsDownloadable { get; set; }
        public Lesson? Lesson { get; set; }
    }

    public enum ResourceType
    {

        Video,
        Pdf,
        Slide,
        Document,
        Link,
        Image,
        Audio
    }
}

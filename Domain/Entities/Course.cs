namespace Domain.Entities
{
    public class Course : Base
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public CourseStatus Status { get; set; } 
        public decimal Price { get; set; }
        public string? RejectReason { get; set; }
        public CourseLevel Level { get; set; }
        public bool IsFree => Price == 0;
        public List<Module>? Modules { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
    }

    public enum CourseStatus
    {
        PendingApproval,
        Published,
        Rejected
    }

    public enum CourseLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }
}

namespace Domain.Entities
{
    public class Enrollment : Base
    {
        public Guid EnrollmentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
        public User? User { get; set; } 
        public Course? Course { get; set; } 
    }
    
    public enum EnrollmentStatus
    {
        Active,
        Completed,
        Cancelled
    }
}

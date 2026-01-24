namespace Domain.Entities
{
    public class Enrollment : Base
    {
        public Guid EnrollmentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
        public decimal ProgressPercent { get; set; } = 0;
        public DateTime? CompletedAt { get; set; }
        public DateTime? EnrolledAt { get; set; }   
        public User? User { get; set; } 
        public Course? Course { get; set; } 
    }
    
    public enum EnrollmentStatus
    {
        PendingPayment,
        Active,
        Completed,
        Cancelled
    }
}

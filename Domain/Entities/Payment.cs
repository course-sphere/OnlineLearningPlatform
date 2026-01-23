namespace Domain.Entities
{
    public class Payment : Base
    {
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public bool IsSuccess { get; set; } = false;
        // Navigation
        public User? User { get; set; }
        public Course? Course { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}

namespace Domain.Entities
{
    public class QuizAttempt : Base
    {
        public Guid QuizAttemptId { get; set; }
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime AttemptedOn { get; set; }
        public decimal Score { get; set; }
        // Navigation
        public Quiz? Quiz { get; set; }
        public User? User { get; set; }
    }
}

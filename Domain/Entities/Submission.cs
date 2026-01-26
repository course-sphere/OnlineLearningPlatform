namespace Domain.Entities
{
    public class Submission : Base
    {
        public Guid SubmissionId { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid UserId { get; set; }
        public string? FileUrl { get; set; }
        public string Content { get; set; }
        public decimal? Score { get; set; } // Điểm số có thể null nếu chưa được chấm
        public User? User { get; set; }
        public Assignment? Assignment { get; set; }  
    }
}

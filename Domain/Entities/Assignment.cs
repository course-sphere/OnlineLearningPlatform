namespace Domain.Entities
{
    public class Assignment : Base
    {
        public Guid AssignmentId { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; } //ngày hết hạn nộp bài
        public decimal MaxScore { get; set; }
        public Course? Course { get; set; }

        public List<Submission>? Submissions { get; set; } 
    }
}

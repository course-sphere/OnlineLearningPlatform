namespace Domain.Entities
{
    public class User : Base
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Image { get; set; }
        public UserRole Role { get; set; }

        public List<Enrollment>? Enrollments { get; set; }
        public List<Payment>? Payments { get; set; }
        public List<QuizAttempt>? Attempts { get; set; }
        public List<Submission>? Submissions { get; set; }  
    }

    public enum UserRole
    {
        Admin,
        Instructor,
        Student
    }
}

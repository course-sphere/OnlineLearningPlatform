namespace Domain.Entities
{
    public class User : Base
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? Image { get; set; }
        public bool IsVerfied { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; } //ex: "Senior Lecturer"
        public List<Enrollment>? Enrollments { get; set; }
        public List<Payment>? Payments { get; set; }
        public List<GradedAttempt>? GradedAttempts { get; set; }
        public List<UserRoleMapping>? Roles { get; set; } = new();
    }
}
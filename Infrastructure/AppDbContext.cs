using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }    
        public DbSet<Course> Courses { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }    
        public DbSet<Enrollment> Enrollments { get; set; }  
        public DbSet<Lesson> Lessons { get; set; }  
        public DbSet<Assignment> Assignments { get; set; }  
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
    }
}

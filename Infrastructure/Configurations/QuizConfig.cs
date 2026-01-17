using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class QuizConfig : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.QuizId);
            builder.HasOne(q => q.Course)
                   .WithMany(c => c.Quizzes)
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

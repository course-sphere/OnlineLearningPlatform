using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class QuizAttemptConfig : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.HasKey(qa => qa.QuizAttemptId);
            builder.HasOne(qa => qa.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(qa => qa.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(qa => qa.User)
                   .WithMany(u => u.Attempts)
                   .HasForeignKey(qa => qa.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

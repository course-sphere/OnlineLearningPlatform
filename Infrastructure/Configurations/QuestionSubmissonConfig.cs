using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class QuestionSubmissonConfig : IEntityTypeConfiguration<QuestionSubmission>
    {
        public void Configure(EntityTypeBuilder<QuestionSubmission> builder)
        {
            builder.HasKey(s => s.QuestionSubmissionId);
            builder.HasOne(s => s.GradedAttempt)
                  .WithMany(a => a.QuestionSubmissions)
                  .HasForeignKey(s => s.GradedAttemptId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Question)
                   .WithMany(q => q.QuestionSubmissions)
                   .HasForeignKey(s => s.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

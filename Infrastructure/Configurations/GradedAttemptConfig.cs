using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GradedAttemptConfig : IEntityTypeConfiguration<GradedAttempt>
    {
        public void Configure(EntityTypeBuilder<GradedAttempt> builder)
        {
            builder.HasKey(qa => qa.GradedAttemptId);
            builder.HasOne(qa => qa.GradedItem)
                   .WithMany(q => q.GradedAttempts)
                   .HasForeignKey(qa => qa.GradedAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(qa => qa.User)
                   .WithMany(u => u.GradedAttempts)
                   .HasForeignKey(qa => qa.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

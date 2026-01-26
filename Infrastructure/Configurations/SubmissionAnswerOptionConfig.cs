using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SubmissionAnswerOptionConfig
        : IEntityTypeConfiguration<SubmissionAnswerOption>
    {
        public void Configure(EntityTypeBuilder<SubmissionAnswerOption> builder)
        {
            // Primary Key
            builder.HasKey(x => x.SubmissionAnswerOptionId);

            // Properties
            builder.Property(x => x.Weight)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(1);

            // Relationship: SubmissionAnswerOption - QuestionSubmission
            builder.HasOne(x => x.QuestionSubmission)
                   .WithMany(s => s.SelectedOptions)
                   .HasForeignKey(x => x.QuestionSubmissionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship: SubmissionAnswerOption - AnswerOption
            builder.HasOne(x => x.AnswerOption)
                   .WithMany(o => o.SubmissionAnswerOptions)
                   .HasForeignKey(x => x.AnswerOptionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

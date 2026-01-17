using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AnswerOptionConfig : IEntityTypeConfiguration<AnswerOption>
    {
        public void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
            builder.HasKey(a => a.AnswerOptionId);

            builder.HasOne(a => a.Question)
                   .WithMany(q => q.AnswerOptions)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }   
}

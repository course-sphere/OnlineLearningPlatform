using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionId);
            builder.HasOne(q => q.GradedItem)
                  .WithMany(g => g.Questions)
                  .HasForeignKey(q => q.GradedItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

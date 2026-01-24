using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class LessonResourceConfig : IEntityTypeConfiguration<LessonResource>
    {
        public void Configure(EntityTypeBuilder<LessonResource> builder)
        {
            builder.HasKey(lr => lr.LessonResourceId);

            builder.HasOne(e => e.Lesson)
               .WithMany(u => u.LessonResources)
               .HasForeignKey(e => e.LessonId)
               .OnDelete(DeleteBehavior.Cascade);   
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class LessonConfig : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.LessonId);
            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(l => l.ParentLesson)
                     .WithMany()
                     .HasForeignKey(l => l.ParentLessonId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

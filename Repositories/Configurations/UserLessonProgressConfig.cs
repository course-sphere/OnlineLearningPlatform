using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations
{
    public class UserLessonProgressConfig : IEntityTypeConfiguration<UserLessonProgress>
    {
        public void Configure(EntityTypeBuilder<UserLessonProgress> builder)
        {
            builder.HasKey(ulp => ulp.LessonProgressId);
            builder.HasOne(e => e.User)
                .WithMany(u => u.UserLessonProgresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Lesson)
                   .WithMany(c => c.UserLessonProgresses)
                   .HasForeignKey(e => e.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

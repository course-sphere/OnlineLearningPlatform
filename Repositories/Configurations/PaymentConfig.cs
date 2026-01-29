using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Course)
                     .WithMany(c => c.Payments)
                     .HasForeignKey(p => p.CourseId)
                     .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Enrollment)
                   .WithMany(e => e.Payments)
                   .HasForeignKey(p => p.EnrollmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

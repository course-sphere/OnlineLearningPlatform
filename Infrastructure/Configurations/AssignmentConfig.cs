using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AssignmentConfig : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.AssignmentId);
            builder.HasOne(a => a.Course)
                   .WithMany(c => c.Assignments)
                   .HasForeignKey(a => a.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

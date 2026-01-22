using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.RoleId);

            builder.HasIndex(r => r.Code).IsUnique();
            builder.HasData(
    new Role
    {
        RoleId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Code = "STUDENT",
        Name = "Student"
    },
    new Role
    {
        RoleId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        Code = "INSTRUCTOR",
        Name = "Instructor"
    },
    new Role
    {
        RoleId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
        Code = "ADMIN",
        Name = "Admin"
    }
);

        }
    }
}

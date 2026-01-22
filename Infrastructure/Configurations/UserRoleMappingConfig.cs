using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Configurations
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.Roles)
                .HasForeignKey(ur => ur.RoleId);
            builder.HasData(
    // Student 1 → STUDENT
    new UserRoleMapping
    {
        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        RoleId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
    },

    // Student 2 → STUDENT
    new UserRoleMapping
    {
        UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        RoleId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
    },

    // Instructor → INSTRUCTOR + STUDENT (vừa dạy vừa học)
    new UserRoleMapping
    {
        UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        RoleId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
    },
    new UserRoleMapping
    {
        UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        RoleId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
    },

    // Admin → ADMIN
    new UserRoleMapping
    {
        UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        RoleId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
    }
);

        }
    }
}

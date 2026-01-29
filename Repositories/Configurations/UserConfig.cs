using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;

namespace Repositories.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.HasIndex(u => u.Email).IsUnique();
            var user1 = CreatePasswordHash("Student1");
            var user2 = CreatePasswordHash("Student2");
            var employer = CreatePasswordHash("Instructor");
            var admin = CreatePasswordHash("Admin");
            var student1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var student2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var instructorId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var adminId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            builder.HasData
                (
                    new User
                    {
                        UserId = student1Id,
                        PasswordHash = user1.PasswordHash,
                        PasswordSalt = user1.PasswordSalt,
                        FullName = "Student1",
                        Email = "Student1@gmail.com",
                        IsVerfied = true
                    },
                    new User
                    {
                        UserId = student2Id,
                        PasswordHash = user2.PasswordHash,
                        PasswordSalt = user2.PasswordSalt,
                        FullName = "Student2",
                        Email = "Student2@gmail.com",
                        IsVerfied = true
                    },
                     new User
                     {
                         UserId = instructorId,
                         PasswordHash = employer.PasswordHash,
                         PasswordSalt = employer.PasswordSalt,
                         FullName = "Instructor",
                         Email = "Employer@gmail.com",
                         IsVerfied = true
                     },
                  new User
                  {
                      UserId = adminId,
                      PasswordHash = admin.PasswordHash,
                      PasswordSalt = admin.PasswordSalt,
                      FullName = "Admin",
                      Email = "Admin@gmail.com",
                      IsVerfied = true
                  }
                );
        }

        private PasswordDTO CreatePasswordHash(string password)
        {
            PasswordDTO pass = new PasswordDTO();
            using (var hmac = new HMACSHA512())
            {
                pass.PasswordSalt = hmac.Key;
                pass.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return pass;
        }
    }
}

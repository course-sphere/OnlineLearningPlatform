using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleAndUserRoleMappingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new DateTime(2026, 1, 22, 15, 50, 19, 854, DateTimeKind.Utc).AddTicks(9767), new byte[] { 21, 235, 95, 93, 39, 14, 75, 198, 64, 106, 23, 115, 251, 218, 157, 161, 42, 155, 192, 244, 73, 97, 63, 140, 39, 24, 180, 246, 159, 53, 43, 217, 128, 71, 129, 2, 178, 253, 208, 100, 62, 64, 8, 66, 131, 237, 109, 60, 242, 101, 145, 167, 136, 218, 252, 17, 35, 108, 215, 36, 167, 225, 247, 26 }, new byte[] { 138, 73, 0, 161, 62, 100, 163, 132, 136, 199, 176, 253, 121, 62, 8, 6, 197, 27, 200, 178, 147, 170, 106, 21, 160, 115, 154, 234, 208, 123, 134, 124, 244, 202, 66, 45, 126, 128, 229, 119, 195, 214, 58, 47, 36, 238, 137, 3, 36, 163, 143, 24, 70, 252, 68, 65, 179, 131, 25, 143, 19, 254, 120, 176, 65, 167, 46, 197, 28, 120, 167, 39, 115, 243, 240, 69, 147, 86, 133, 200, 102, 65, 186, 97, 53, 56, 237, 169, 192, 85, 2, 50, 130, 224, 4, 43, 140, 184, 178, 144, 37, 241, 252, 177, 60, 15, 225, 138, 125, 186, 98, 128, 71, 208, 77, 63, 28, 19, 90, 148, 59, 241, 169, 135, 45, 82, 66, 9 }, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new DateTime(2026, 1, 22, 15, 50, 19, 854, DateTimeKind.Utc).AddTicks(9773), new byte[] { 163, 124, 213, 223, 134, 59, 200, 187, 97, 134, 82, 24, 95, 24, 55, 197, 176, 94, 175, 82, 5, 216, 65, 66, 16, 99, 46, 243, 1, 4, 129, 221, 236, 240, 104, 31, 125, 64, 236, 141, 6, 166, 200, 70, 147, 92, 6, 173, 136, 59, 152, 17, 177, 72, 220, 104, 221, 55, 26, 28, 234, 172, 74, 128 }, new byte[] { 61, 178, 164, 20, 193, 33, 188, 65, 110, 106, 110, 176, 245, 37, 29, 63, 162, 14, 47, 152, 240, 195, 204, 109, 47, 207, 20, 64, 190, 118, 63, 139, 21, 8, 32, 35, 120, 185, 212, 242, 121, 62, 103, 22, 163, 59, 241, 230, 235, 206, 189, 5, 51, 86, 182, 54, 228, 125, 213, 105, 209, 138, 32, 32, 217, 134, 29, 208, 145, 243, 18, 82, 9, 123, 196, 58, 241, 181, 24, 109, 245, 137, 84, 155, 108, 58, 39, 221, 172, 199, 80, 151, 128, 103, 191, 74, 62, 219, 192, 143, 243, 110, 94, 200, 184, 93, 174, 96, 6, 172, 131, 171, 93, 129, 23, 26, 151, 159, 111, 164, 53, 22, 7, 98, 173, 43, 169, 155 }, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new DateTime(2026, 1, 22, 15, 50, 19, 854, DateTimeKind.Utc).AddTicks(9775), new byte[] { 164, 74, 96, 80, 40, 138, 19, 45, 59, 109, 178, 214, 227, 113, 131, 222, 241, 162, 38, 140, 171, 169, 114, 126, 26, 153, 8, 61, 115, 156, 53, 53, 210, 86, 233, 109, 157, 118, 218, 194, 78, 68, 78, 105, 221, 208, 106, 91, 135, 135, 164, 220, 234, 82, 157, 237, 108, 50, 251, 121, 247, 217, 249, 231 }, new byte[] { 151, 213, 87, 53, 169, 126, 193, 6, 193, 166, 14, 53, 93, 148, 232, 26, 162, 166, 20, 225, 209, 69, 120, 89, 104, 158, 76, 86, 212, 222, 201, 156, 186, 101, 187, 45, 19, 61, 2, 202, 249, 213, 211, 143, 132, 136, 254, 178, 15, 53, 72, 180, 136, 102, 239, 149, 165, 20, 92, 4, 118, 67, 135, 219, 138, 221, 200, 59, 190, 191, 184, 142, 231, 221, 123, 187, 181, 18, 19, 130, 107, 30, 58, 194, 129, 226, 110, 106, 199, 185, 188, 94, 252, 22, 118, 70, 93, 248, 67, 44, 128, 129, 113, 1, 13, 190, 61, 21, 144, 9, 249, 147, 206, 108, 141, 49, 8, 15, 249, 89, 214, 61, 116, 178, 87, 216, 99, 171 }, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new DateTime(2026, 1, 22, 15, 50, 19, 854, DateTimeKind.Utc).AddTicks(9777), new byte[] { 30, 227, 159, 26, 167, 45, 104, 65, 136, 129, 161, 195, 111, 113, 211, 245, 201, 77, 243, 28, 59, 169, 102, 12, 118, 4, 190, 70, 202, 63, 12, 228, 21, 16, 164, 163, 56, 178, 83, 8, 237, 83, 252, 252, 35, 243, 174, 178, 23, 158, 224, 237, 93, 177, 17, 234, 195, 252, 129, 162, 48, 121, 18, 69 }, new byte[] { 199, 187, 100, 62, 34, 211, 229, 241, 9, 217, 176, 25, 181, 17, 3, 150, 106, 40, 169, 127, 179, 53, 85, 69, 199, 175, 108, 99, 113, 11, 239, 2, 228, 98, 105, 136, 186, 152, 132, 119, 174, 27, 147, 248, 85, 46, 186, 171, 36, 69, 71, 8, 221, 2, 226, 0, 134, 67, 90, 52, 231, 236, 41, 218, 171, 172, 47, 230, 172, 243, 247, 140, 136, 64, 79, 161, 151, 210, 75, 218, 34, 56, 123, 72, 44, 246, 30, 179, 145, 158, 21, 167, 78, 230, 220, 89, 29, 155, 11, 70, 207, 39, 112, 4, 150, 158, 183, 193, 175, 188, 238, 30, 7, 83, 246, 59, 20, 117, 43, 182, 142, 173, 69, 225, 238, 75, 103, 39 }, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMappings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMappings", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "STUDENT", "Student" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "INSTRUCTOR", "Instructor" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "ADMIN", "Admin" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5551), new byte[] { 252, 31, 85, 113, 161, 243, 176, 196, 228, 104, 124, 91, 210, 152, 63, 43, 97, 78, 112, 26, 237, 131, 160, 146, 124, 54, 53, 216, 235, 86, 237, 15, 1, 6, 161, 93, 13, 105, 186, 60, 141, 144, 16, 56, 151, 140, 224, 140, 206, 74, 63, 49, 233, 190, 56, 163, 14, 224, 71, 95, 70, 45, 132, 125 }, new byte[] { 57, 200, 123, 30, 60, 63, 163, 50, 87, 185, 246, 15, 230, 72, 69, 134, 229, 147, 205, 28, 83, 154, 147, 6, 171, 205, 151, 208, 71, 54, 182, 69, 41, 76, 131, 89, 28, 77, 173, 214, 61, 217, 188, 222, 66, 177, 123, 23, 77, 19, 246, 234, 172, 103, 241, 170, 28, 211, 123, 9, 236, 208, 123, 187, 207, 85, 185, 125, 191, 122, 188, 26, 169, 82, 79, 112, 108, 117, 66, 134, 228, 194, 59, 149, 192, 70, 21, 109, 48, 242, 98, 218, 157, 255, 19, 34, 127, 128, 202, 39, 129, 168, 131, 71, 146, 218, 53, 195, 115, 173, 175, 232, 231, 54, 115, 24, 211, 169, 127, 158, 213, 246, 177, 64, 248, 159, 29, 107 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5619), new byte[] { 158, 139, 237, 129, 89, 104, 117, 36, 247, 175, 183, 217, 69, 236, 26, 119, 171, 150, 183, 106, 226, 33, 3, 95, 60, 96, 150, 54, 196, 65, 149, 195, 183, 116, 10, 11, 110, 215, 115, 165, 247, 195, 62, 24, 209, 80, 209, 239, 134, 245, 92, 24, 67, 199, 207, 200, 186, 49, 58, 246, 42, 198, 121, 117 }, new byte[] { 32, 107, 99, 51, 162, 76, 70, 196, 12, 249, 223, 94, 154, 22, 255, 232, 221, 170, 116, 166, 26, 189, 173, 97, 207, 209, 191, 124, 124, 179, 226, 234, 206, 152, 203, 109, 188, 114, 9, 157, 159, 213, 32, 176, 214, 154, 23, 187, 193, 121, 168, 160, 71, 23, 197, 214, 102, 117, 63, 68, 151, 249, 118, 228, 121, 19, 3, 167, 60, 165, 10, 103, 172, 147, 99, 34, 246, 95, 224, 152, 185, 203, 21, 43, 238, 28, 146, 176, 159, 23, 246, 52, 214, 82, 43, 255, 43, 91, 235, 111, 114, 57, 81, 189, 50, 227, 237, 95, 47, 73, 127, 113, 165, 224, 157, 115, 133, 16, 5, 216, 70, 180, 85, 245, 183, 254, 138, 255 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5623), new byte[] { 128, 183, 60, 61, 222, 191, 218, 251, 12, 147, 191, 24, 48, 45, 92, 185, 88, 49, 145, 27, 224, 112, 151, 194, 225, 112, 102, 15, 34, 107, 248, 97, 141, 208, 191, 183, 63, 34, 137, 26, 45, 7, 7, 223, 110, 10, 218, 74, 38, 21, 41, 56, 247, 45, 64, 249, 197, 109, 34, 22, 152, 38, 41, 214 }, new byte[] { 236, 12, 168, 205, 42, 92, 198, 73, 157, 68, 190, 124, 110, 12, 189, 217, 137, 99, 128, 110, 141, 206, 241, 130, 113, 60, 55, 106, 43, 172, 203, 48, 18, 97, 129, 128, 100, 90, 110, 71, 26, 4, 83, 56, 218, 237, 184, 43, 22, 53, 84, 204, 16, 21, 33, 125, 86, 135, 45, 96, 107, 173, 179, 6, 5, 99, 173, 212, 121, 150, 213, 51, 187, 206, 187, 89, 68, 253, 134, 251, 204, 139, 74, 182, 24, 222, 81, 190, 164, 206, 136, 187, 17, 199, 227, 183, 56, 17, 209, 116, 176, 197, 3, 34, 251, 72, 134, 248, 252, 9, 147, 152, 100, 20, 12, 213, 112, 150, 115, 72, 190, 85, 49, 203, 59, 193, 243, 229 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5625), new byte[] { 60, 23, 8, 11, 174, 255, 37, 214, 143, 49, 177, 162, 83, 115, 225, 56, 131, 201, 210, 24, 226, 217, 59, 61, 82, 231, 156, 62, 16, 73, 76, 131, 2, 35, 36, 211, 187, 167, 0, 207, 9, 168, 71, 67, 130, 157, 174, 17, 68, 248, 203, 10, 70, 80, 41, 108, 246, 248, 19, 58, 228, 30, 188, 108 }, new byte[] { 130, 190, 216, 129, 26, 171, 238, 89, 119, 158, 130, 37, 210, 128, 50, 146, 154, 65, 66, 40, 90, 127, 173, 137, 116, 14, 127, 215, 213, 90, 222, 137, 136, 21, 111, 83, 191, 204, 94, 246, 18, 232, 88, 190, 47, 122, 227, 25, 226, 4, 169, 241, 246, 231, 53, 87, 21, 203, 180, 16, 94, 92, 240, 183, 125, 66, 28, 205, 35, 150, 184, 216, 190, 46, 193, 170, 4, 96, 152, 255, 66, 181, 132, 125, 220, 123, 142, 139, 79, 137, 205, 193, 250, 31, 183, 221, 241, 127, 238, 235, 252, 142, 16, 121, 129, 18, 174, 173, 23, 240, 79, 226, 153, 218, 104, 254, 119, 25, 204, 200, 178, 72, 60, 86, 48, 213, 226, 250 } });

            migrationBuilder.InsertData(
                table: "UserRoleMappings",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_RoleId",
                table: "UserRoleMappings",
                column: "RoleId");
        }
    }
}

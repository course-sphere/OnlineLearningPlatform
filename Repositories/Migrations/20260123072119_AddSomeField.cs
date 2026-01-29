using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Enrollments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrolledAt",
                table: "Enrollments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProgressPercent",
                table: "Enrollments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "UserLessonProgresses",
                columns: table => new
                {
                    LessonProgressId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastWatchedSecond = table.Column<int>(type: "integer", nullable: true),
                    CompletionPercent = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLessonProgresses", x => x.LessonProgressId);
                    table.ForeignKey(
                        name: "FK_UserLessonProgresses_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLessonProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 7, 21, 18, 910, DateTimeKind.Utc).AddTicks(9846), new byte[] { 85, 50, 55, 157, 144, 195, 134, 35, 49, 82, 45, 166, 213, 64, 95, 203, 75, 59, 221, 34, 178, 148, 241, 80, 36, 132, 41, 149, 110, 113, 242, 57, 127, 58, 249, 59, 148, 15, 14, 155, 46, 137, 236, 130, 83, 19, 187, 110, 93, 95, 5, 120, 200, 178, 235, 5, 245, 1, 105, 120, 44, 245, 15, 231 }, new byte[] { 34, 231, 193, 99, 4, 77, 170, 132, 7, 187, 196, 43, 132, 103, 26, 182, 226, 149, 110, 246, 177, 45, 186, 96, 143, 112, 126, 192, 14, 176, 243, 27, 128, 54, 71, 82, 125, 72, 161, 148, 71, 85, 50, 173, 34, 209, 100, 236, 192, 176, 26, 185, 221, 121, 231, 14, 83, 216, 75, 100, 114, 230, 189, 94, 70, 180, 2, 45, 227, 51, 253, 239, 40, 239, 208, 250, 111, 130, 38, 215, 178, 174, 110, 71, 91, 98, 235, 101, 160, 97, 9, 49, 221, 18, 114, 214, 23, 115, 81, 235, 24, 63, 222, 139, 27, 1, 54, 9, 155, 191, 45, 109, 190, 4, 130, 4, 89, 32, 94, 87, 100, 118, 110, 204, 132, 229, 14, 229 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 7, 21, 18, 910, DateTimeKind.Utc).AddTicks(9852), new byte[] { 13, 218, 5, 159, 190, 139, 190, 112, 166, 60, 80, 215, 220, 151, 113, 191, 13, 216, 163, 128, 71, 137, 248, 199, 39, 69, 242, 113, 214, 234, 42, 124, 140, 207, 199, 232, 182, 10, 247, 194, 62, 42, 44, 236, 154, 47, 182, 75, 24, 122, 52, 117, 75, 255, 2, 189, 129, 17, 130, 88, 81, 144, 216, 108 }, new byte[] { 87, 229, 46, 45, 193, 49, 213, 157, 76, 150, 42, 57, 60, 115, 26, 209, 62, 181, 205, 107, 22, 175, 249, 118, 1, 21, 125, 76, 67, 28, 23, 148, 86, 8, 13, 102, 192, 150, 4, 5, 215, 138, 44, 71, 2, 209, 44, 175, 31, 19, 112, 243, 65, 27, 128, 35, 94, 189, 104, 139, 122, 90, 5, 112, 230, 180, 52, 15, 233, 25, 57, 220, 177, 46, 141, 162, 219, 139, 111, 227, 43, 195, 12, 118, 230, 85, 84, 208, 37, 116, 251, 113, 120, 2, 77, 217, 2, 58, 210, 82, 43, 32, 103, 145, 118, 219, 33, 51, 53, 235, 95, 112, 116, 223, 202, 214, 10, 229, 129, 35, 245, 239, 239, 159, 219, 109, 209, 146 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 7, 21, 18, 910, DateTimeKind.Utc).AddTicks(9854), new byte[] { 6, 81, 144, 90, 165, 230, 238, 255, 76, 135, 228, 177, 226, 179, 120, 194, 173, 38, 216, 31, 69, 185, 207, 242, 44, 98, 254, 108, 188, 157, 66, 128, 240, 95, 158, 92, 103, 7, 95, 206, 49, 15, 154, 187, 142, 209, 126, 103, 119, 50, 162, 136, 212, 226, 68, 81, 171, 34, 130, 224, 82, 182, 78, 35 }, new byte[] { 158, 216, 55, 145, 186, 186, 215, 89, 237, 79, 200, 47, 155, 61, 74, 166, 30, 27, 16, 228, 158, 94, 7, 45, 26, 78, 76, 169, 242, 108, 244, 78, 247, 10, 56, 106, 122, 237, 184, 105, 124, 107, 217, 82, 50, 136, 96, 101, 191, 127, 130, 11, 229, 179, 117, 108, 117, 44, 97, 113, 30, 162, 240, 103, 180, 36, 108, 237, 122, 116, 255, 225, 106, 85, 69, 181, 44, 81, 210, 124, 148, 174, 62, 181, 211, 107, 30, 185, 236, 127, 34, 90, 189, 31, 233, 70, 129, 58, 190, 149, 91, 19, 223, 95, 128, 73, 38, 128, 98, 176, 178, 212, 191, 127, 7, 151, 6, 165, 37, 220, 26, 11, 121, 128, 71, 119, 164, 229 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 7, 21, 18, 910, DateTimeKind.Utc).AddTicks(9856), new byte[] { 76, 231, 123, 87, 236, 97, 118, 27, 43, 3, 116, 5, 47, 153, 45, 165, 108, 220, 52, 149, 171, 64, 177, 53, 111, 47, 42, 37, 11, 129, 34, 148, 157, 215, 187, 239, 156, 16, 155, 172, 150, 217, 42, 73, 231, 83, 4, 255, 85, 236, 32, 120, 21, 188, 213, 93, 28, 89, 201, 37, 101, 123, 94, 183 }, new byte[] { 30, 117, 169, 59, 54, 176, 230, 205, 227, 100, 65, 255, 54, 42, 208, 110, 59, 165, 0, 68, 169, 173, 246, 106, 151, 1, 166, 115, 239, 3, 140, 2, 42, 35, 14, 237, 153, 173, 162, 244, 205, 254, 145, 242, 172, 115, 177, 200, 206, 112, 253, 245, 188, 80, 160, 67, 214, 239, 46, 187, 235, 159, 223, 213, 219, 35, 60, 164, 15, 238, 193, 131, 12, 102, 182, 21, 246, 74, 220, 213, 88, 187, 75, 132, 158, 185, 164, 120, 214, 56, 116, 123, 139, 32, 161, 71, 126, 191, 82, 3, 55, 75, 16, 227, 13, 108, 126, 101, 52, 157, 94, 69, 60, 148, 104, 103, 119, 49, 55, 235, 53, 164, 247, 215, 59, 10, 216, 2 } });

            migrationBuilder.CreateIndex(
                name: "IX_UserLessonProgresses_LessonId",
                table: "UserLessonProgresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessonProgresses_UserId",
                table: "UserLessonProgresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLessonProgresses");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "EnrolledAt",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "ProgressPercent",
                table: "Enrollments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 6, 46, 52, 887, DateTimeKind.Utc).AddTicks(2021), new byte[] { 229, 108, 253, 57, 176, 132, 18, 154, 90, 114, 124, 204, 96, 29, 130, 243, 199, 213, 141, 41, 151, 14, 57, 226, 161, 40, 241, 138, 146, 11, 185, 69, 136, 130, 222, 37, 6, 116, 42, 242, 122, 70, 4, 250, 132, 92, 119, 85, 75, 87, 15, 85, 60, 233, 255, 115, 149, 126, 104, 86, 87, 85, 13, 131 }, new byte[] { 53, 161, 190, 29, 169, 222, 205, 142, 17, 47, 237, 84, 228, 29, 237, 230, 161, 16, 58, 107, 19, 109, 28, 28, 232, 151, 186, 87, 248, 198, 18, 51, 17, 142, 172, 92, 182, 125, 229, 217, 55, 247, 31, 176, 57, 212, 147, 97, 217, 139, 111, 167, 41, 21, 88, 249, 241, 44, 94, 254, 247, 209, 22, 178, 199, 194, 164, 171, 229, 101, 220, 94, 29, 113, 0, 181, 93, 191, 71, 203, 71, 22, 157, 2, 89, 178, 197, 141, 139, 171, 99, 114, 168, 212, 137, 174, 255, 132, 21, 59, 49, 113, 71, 193, 249, 220, 224, 220, 252, 147, 192, 134, 108, 22, 249, 255, 162, 102, 221, 151, 111, 0, 69, 97, 254, 139, 8, 30 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 6, 46, 52, 887, DateTimeKind.Utc).AddTicks(2029), new byte[] { 96, 115, 174, 110, 43, 80, 114, 228, 224, 124, 160, 124, 88, 108, 80, 180, 26, 48, 172, 76, 239, 112, 172, 169, 138, 229, 201, 33, 213, 19, 89, 83, 110, 190, 252, 192, 53, 63, 189, 55, 218, 153, 213, 86, 247, 68, 84, 22, 198, 248, 111, 231, 241, 68, 239, 217, 50, 118, 24, 59, 6, 63, 129, 12 }, new byte[] { 224, 238, 150, 74, 34, 13, 47, 237, 176, 149, 167, 114, 193, 101, 48, 212, 139, 255, 150, 225, 130, 95, 55, 163, 188, 160, 160, 199, 185, 227, 185, 174, 138, 198, 35, 147, 169, 149, 137, 244, 132, 254, 1, 226, 243, 114, 73, 65, 199, 81, 140, 14, 55, 73, 253, 169, 242, 138, 66, 85, 216, 254, 113, 224, 215, 64, 49, 40, 53, 81, 175, 77, 156, 32, 253, 193, 57, 14, 73, 225, 253, 174, 46, 223, 2, 251, 227, 100, 61, 141, 59, 121, 222, 91, 90, 195, 185, 105, 206, 125, 100, 43, 116, 38, 172, 71, 176, 220, 125, 194, 147, 39, 68, 224, 10, 54, 103, 180, 162, 139, 138, 8, 201, 0, 219, 208, 53, 178 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 6, 46, 52, 887, DateTimeKind.Utc).AddTicks(2032), new byte[] { 40, 188, 130, 173, 91, 61, 41, 125, 17, 125, 205, 148, 62, 21, 255, 205, 121, 112, 131, 84, 186, 17, 222, 251, 50, 191, 142, 139, 63, 229, 25, 118, 159, 188, 54, 166, 90, 94, 191, 43, 69, 48, 152, 54, 178, 127, 200, 111, 68, 96, 239, 74, 236, 6, 233, 84, 32, 161, 32, 195, 175, 16, 142, 216 }, new byte[] { 74, 32, 195, 72, 150, 240, 247, 110, 166, 103, 252, 232, 245, 203, 245, 144, 242, 120, 177, 27, 208, 228, 249, 200, 201, 201, 11, 217, 179, 36, 104, 218, 74, 248, 247, 242, 55, 12, 11, 110, 77, 98, 221, 59, 164, 127, 33, 206, 14, 193, 18, 97, 147, 125, 124, 232, 7, 91, 34, 32, 114, 212, 4, 56, 203, 246, 230, 230, 40, 106, 40, 224, 26, 88, 103, 7, 156, 220, 61, 210, 248, 26, 15, 176, 6, 62, 18, 53, 206, 87, 106, 127, 19, 122, 132, 244, 4, 4, 253, 11, 117, 232, 215, 235, 6, 55, 44, 17, 179, 23, 31, 168, 35, 94, 31, 116, 79, 203, 240, 228, 150, 60, 222, 73, 180, 1, 180, 247 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 23, 6, 46, 52, 887, DateTimeKind.Utc).AddTicks(2034), new byte[] { 73, 113, 154, 122, 106, 228, 155, 247, 181, 172, 139, 92, 234, 3, 9, 98, 221, 47, 133, 80, 54, 109, 149, 116, 183, 221, 238, 221, 183, 92, 218, 96, 135, 37, 192, 241, 89, 241, 146, 84, 28, 205, 22, 174, 209, 214, 114, 136, 37, 101, 103, 130, 225, 156, 61, 125, 184, 93, 175, 97, 6, 134, 64, 39 }, new byte[] { 171, 82, 251, 110, 102, 52, 128, 218, 163, 17, 117, 13, 73, 90, 209, 244, 194, 233, 37, 222, 154, 106, 197, 237, 99, 243, 56, 52, 252, 129, 255, 96, 100, 217, 85, 201, 45, 223, 132, 122, 11, 185, 128, 130, 145, 1, 134, 37, 4, 156, 98, 223, 207, 147, 26, 164, 14, 103, 156, 182, 80, 171, 100, 174, 233, 8, 245, 143, 241, 139, 154, 236, 186, 165, 207, 190, 7, 130, 166, 153, 1, 200, 70, 48, 90, 78, 164, 246, 10, 43, 187, 136, 90, 14, 164, 125, 129, 59, 16, 103, 197, 140, 174, 236, 61, 103, 233, 165, 32, 65, 126, 11, 199, 108, 217, 105, 143, 216, 113, 45, 71, 220, 224, 204, 248, 247, 55, 221 } });
        }
    }
}

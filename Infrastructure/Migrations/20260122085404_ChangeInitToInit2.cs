using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInitToInit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_ParentLessonId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ParentLessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentLessonId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "Questions",
                newName: "GradedItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                newName: "IX_Questions_GradedItemId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Lessons",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                newName: "IX_Lessons_ModuleId");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Courses",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AnswerOptions",
                newName: "OrderIndex");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Questions",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Index",
                table: "Lessons",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedMinutes",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGraded",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Enrollments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "Courses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "AnswerOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "AnswerOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AnswerOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "AnswerOptions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "GradedItems",
                columns: table => new
                {
                    GradedItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    IsAutoGraded = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedItems", x => x.GradedItemId);
                    table.ForeignKey(
                        name: "FK_GradedItems_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                    table.ForeignKey(
                        name: "FK_Modules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "GradedAttempts",
                columns: table => new
                {
                    GradedAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GradedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedAttempts", x => x.GradedAttemptId);
                    table.ForeignKey(
                        name: "FK_GradedAttempts_GradedItems_GradedAttemptId",
                        column: x => x.GradedAttemptId,
                        principalTable: "GradedItems",
                        principalColumn: "GradedItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradedAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "QuestionSubmissions",
                columns: table => new
                {
                    QuestionSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    IsAutoGraded = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSubmissions", x => x.QuestionSubmissionId);
                    table.ForeignKey(
                        name: "FK_QuestionSubmissions_GradedAttempts_GradedAttemptId",
                        column: x => x.GradedAttemptId,
                        principalTable: "GradedAttempts",
                        principalColumn: "GradedAttemptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionAnswerOptions",
                columns: table => new
                {
                    SubmissionAnswerOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 1m),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionAnswerOptions", x => x.SubmissionAnswerOptionId);
                    table.ForeignKey(
                        name: "FK_SubmissionAnswerOptions_AnswerOptions_AnswerOptionId",
                        column: x => x.AnswerOptionId,
                        principalTable: "AnswerOptions",
                        principalColumn: "AnswerOptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubmissionAnswerOptions_QuestionSubmissions_QuestionSubmiss~",
                        column: x => x.QuestionSubmissionId,
                        principalTable: "QuestionSubmissions",
                        principalColumn: "QuestionSubmissionId",
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "CreatedAt", "CreatedBy", "Email", "FullName", "Image", "IsDeleted", "IsVerfied", "PasswordHash", "PasswordSalt", "PhoneNumber", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5551), new Guid("00000000-0000-0000-0000-000000000000"), "Student1@gmail.com", "Student1", null, false, true, new byte[] { 252, 31, 85, 113, 161, 243, 176, 196, 228, 104, 124, 91, 210, 152, 63, 43, 97, 78, 112, 26, 237, 131, 160, 146, 124, 54, 53, 216, 235, 86, 237, 15, 1, 6, 161, 93, 13, 105, 186, 60, 141, 144, 16, 56, 151, 140, 224, 140, 206, 74, 63, 49, 233, 190, 56, 163, 14, 224, 71, 95, 70, 45, 132, 125 }, new byte[] { 57, 200, 123, 30, 60, 63, 163, 50, 87, 185, 246, 15, 230, 72, 69, 134, 229, 147, 205, 28, 83, 154, 147, 6, 171, 205, 151, 208, 71, 54, 182, 69, 41, 76, 131, 89, 28, 77, 173, 214, 61, 217, 188, 222, 66, 177, 123, 23, 77, 19, 246, 234, 172, 103, 241, 170, 28, 211, 123, 9, 236, 208, 123, 187, 207, 85, 185, 125, 191, 122, 188, 26, 169, 82, 79, 112, 108, 117, 66, 134, 228, 194, 59, 149, 192, 70, 21, 109, 48, 242, 98, 218, 157, 255, 19, 34, 127, 128, 202, 39, 129, 168, 131, 71, 146, 218, 53, 195, 115, 173, 175, 232, 231, 54, 115, 24, 211, 169, 127, 158, 213, 246, 177, 64, 248, 159, 29, 107 }, null, null, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5619), new Guid("00000000-0000-0000-0000-000000000000"), "Student2@gmail.com", "Student2", null, false, true, new byte[] { 158, 139, 237, 129, 89, 104, 117, 36, 247, 175, 183, 217, 69, 236, 26, 119, 171, 150, 183, 106, 226, 33, 3, 95, 60, 96, 150, 54, 196, 65, 149, 195, 183, 116, 10, 11, 110, 215, 115, 165, 247, 195, 62, 24, 209, 80, 209, 239, 134, 245, 92, 24, 67, 199, 207, 200, 186, 49, 58, 246, 42, 198, 121, 117 }, new byte[] { 32, 107, 99, 51, 162, 76, 70, 196, 12, 249, 223, 94, 154, 22, 255, 232, 221, 170, 116, 166, 26, 189, 173, 97, 207, 209, 191, 124, 124, 179, 226, 234, 206, 152, 203, 109, 188, 114, 9, 157, 159, 213, 32, 176, 214, 154, 23, 187, 193, 121, 168, 160, 71, 23, 197, 214, 102, 117, 63, 68, 151, 249, 118, 228, 121, 19, 3, 167, 60, 165, 10, 103, 172, 147, 99, 34, 246, 95, 224, 152, 185, 203, 21, 43, 238, 28, 146, 176, 159, 23, 246, 52, 214, 82, 43, 255, 43, 91, 235, 111, 114, 57, 81, 189, 50, 227, 237, 95, 47, 73, 127, 113, 165, 224, 157, 115, 133, 16, 5, 216, 70, 180, 85, 245, 183, 254, 138, 255 }, null, null, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5623), new Guid("00000000-0000-0000-0000-000000000000"), "Employer@gmail.com", "Instructor", null, false, true, new byte[] { 128, 183, 60, 61, 222, 191, 218, 251, 12, 147, 191, 24, 48, 45, 92, 185, 88, 49, 145, 27, 224, 112, 151, 194, 225, 112, 102, 15, 34, 107, 248, 97, 141, 208, 191, 183, 63, 34, 137, 26, 45, 7, 7, 223, 110, 10, 218, 74, 38, 21, 41, 56, 247, 45, 64, 249, 197, 109, 34, 22, 152, 38, 41, 214 }, new byte[] { 236, 12, 168, 205, 42, 92, 198, 73, 157, 68, 190, 124, 110, 12, 189, 217, 137, 99, 128, 110, 141, 206, 241, 130, 113, 60, 55, 106, 43, 172, 203, 48, 18, 97, 129, 128, 100, 90, 110, 71, 26, 4, 83, 56, 218, 237, 184, 43, 22, 53, 84, 204, 16, 21, 33, 125, 86, 135, 45, 96, 107, 173, 179, 6, 5, 99, 173, 212, 121, 150, 213, 51, 187, 206, 187, 89, 68, 253, 134, 251, 204, 139, 74, 182, 24, 222, 81, 190, 164, 206, 136, 187, 17, 199, 227, 183, 56, 17, 209, 116, 176, 197, 3, 34, 251, 72, 134, 248, 252, 9, 147, 152, 100, 20, 12, 213, 112, 150, 115, 72, 190, 85, 49, 203, 59, 193, 243, 229 }, null, null, null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, new DateTime(2026, 1, 22, 8, 54, 2, 196, DateTimeKind.Utc).AddTicks(5625), new Guid("00000000-0000-0000-0000-000000000000"), "Admin@gmail.com", "Admin", null, false, true, new byte[] { 60, 23, 8, 11, 174, 255, 37, 214, 143, 49, 177, 162, 83, 115, 225, 56, 131, 201, 210, 24, 226, 217, 59, 61, 82, 231, 156, 62, 16, 73, 76, 131, 2, 35, 36, 211, 187, 167, 0, 207, 9, 168, 71, 67, 130, 157, 174, 17, 68, 248, 203, 10, 70, 80, 41, 108, 246, 248, 19, 58, 228, 30, 188, 108 }, new byte[] { 130, 190, 216, 129, 26, 171, 238, 89, 119, 158, 130, 37, 210, 128, 50, 146, 154, 65, 66, 40, 90, 127, 173, 137, 116, 14, 127, 215, 213, 90, 222, 137, 136, 21, 111, 83, 191, 204, 94, 246, 18, 232, 88, 190, 47, 122, 227, 25, 226, 4, 169, 241, 246, 231, 53, 87, 21, 203, 180, 16, 94, 92, 240, 183, 125, 66, 28, 205, 35, 150, 184, 216, 190, 46, 193, 170, 4, 96, 152, 255, 66, 181, 132, 125, 220, 123, 142, 139, 79, 137, 205, 193, 250, 31, 183, 221, 241, 127, 238, 235, 252, 142, 16, 121, 129, 18, 174, 173, 23, 240, 79, 226, 153, 218, 104, 254, 119, 25, 204, 200, 178, 72, 60, 86, 48, 213, 226, 250 }, null, null, null, null }
                });

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
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradedAttempts_UserId",
                table: "GradedAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedItems_LessonId",
                table: "GradedItems",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSubmissions_GradedAttemptId",
                table: "QuestionSubmissions",
                column: "GradedAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSubmissions_QuestionId",
                table: "QuestionSubmissions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswerOptions_AnswerOptionId",
                table: "SubmissionAnswerOptions",
                column: "AnswerOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswerOptions_QuestionSubmissionId",
                table: "SubmissionAnswerOptions",
                column: "QuestionSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_RoleId",
                table: "UserRoleMappings",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_GradedItems_GradedItemId",
                table: "Questions",
                column: "GradedItemId",
                principalTable: "GradedItems",
                principalColumn: "GradedItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_GradedItems_GradedItemId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "SubmissionAnswerOptions");

            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "QuestionSubmissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "GradedAttempts");

            migrationBuilder.DropTable(
                name: "GradedItems");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EstimatedMinutes",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsGraded",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AnswerOptions");

            migrationBuilder.RenameColumn(
                name: "GradedItemId",
                table: "Questions",
                newName: "QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_GradedItemId",
                table: "Questions",
                newName: "IX_Questions_QuizId");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Lessons",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ModuleId",
                table: "Lessons",
                newName: "IX_Lessons_CourseId");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Courses",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "OrderIndex",
                table: "AnswerOptions",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Index",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonId",
                table: "Lessons",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeLimit = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TotalPoint = table.Column<decimal>(type: "numeric", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    SubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAttemptId);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ParentLessonId",
                table: "Lessons",
                column: "ParentLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_UserId",
                table: "QuizAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CourseId",
                table: "Quizzes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_UserId",
                table: "Submissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_ParentLessonId",
                table: "Lessons",
                column: "ParentLessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

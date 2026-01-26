using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeHuyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionSubmissionId1",
                table: "SubmissionAnswerOptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "QuestionSubmissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "GradedAttempts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    PendingBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalEarnings = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalWithdrawn = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    WalletTransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.WalletTransactionId);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 26, 13, 31, 46, 959, DateTimeKind.Utc).AddTicks(8960), new byte[] { 42, 157, 231, 164, 96, 76, 195, 123, 134, 115, 39, 246, 200, 108, 243, 242, 219, 203, 241, 117, 56, 141, 219, 116, 124, 248, 56, 142, 248, 16, 224, 213, 113, 69, 39, 177, 170, 254, 99, 131, 236, 186, 196, 77, 162, 85, 203, 220, 215, 172, 14, 142, 158, 50, 197, 65, 37, 169, 221, 125, 115, 205, 172, 51 }, new byte[] { 184, 81, 50, 223, 114, 147, 213, 243, 125, 188, 83, 229, 15, 42, 85, 69, 245, 249, 234, 144, 210, 186, 228, 216, 56, 143, 75, 227, 151, 201, 60, 140, 102, 169, 114, 173, 91, 106, 32, 90, 181, 100, 255, 187, 157, 100, 59, 3, 146, 181, 74, 135, 230, 106, 177, 28, 176, 39, 112, 221, 124, 11, 79, 154, 45, 112, 107, 81, 116, 217, 244, 82, 253, 201, 42, 216, 129, 52, 84, 7, 114, 102, 7, 96, 150, 183, 160, 21, 22, 38, 252, 210, 37, 56, 43, 35, 167, 82, 100, 154, 214, 34, 59, 46, 33, 99, 8, 168, 193, 202, 26, 146, 106, 4, 116, 52, 222, 1, 133, 189, 56, 23, 204, 198, 24, 47, 253, 27 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 26, 13, 31, 46, 959, DateTimeKind.Utc).AddTicks(8965), new byte[] { 174, 214, 181, 11, 126, 43, 59, 151, 192, 163, 188, 100, 134, 62, 142, 16, 42, 111, 210, 186, 167, 145, 241, 65, 16, 66, 18, 205, 56, 74, 156, 135, 28, 128, 200, 189, 213, 215, 117, 193, 140, 158, 221, 33, 181, 229, 20, 241, 29, 79, 91, 17, 71, 189, 18, 113, 230, 153, 63, 249, 5, 55, 186, 129 }, new byte[] { 81, 197, 192, 221, 61, 199, 65, 224, 221, 217, 91, 167, 6, 39, 219, 240, 173, 4, 220, 251, 8, 248, 95, 10, 60, 85, 19, 82, 93, 234, 88, 61, 242, 189, 83, 147, 25, 210, 202, 250, 205, 190, 82, 149, 143, 156, 138, 34, 209, 33, 88, 35, 130, 194, 206, 152, 164, 49, 113, 190, 119, 232, 181, 239, 44, 66, 195, 178, 196, 104, 0, 197, 64, 40, 49, 42, 246, 122, 178, 167, 86, 68, 137, 79, 21, 19, 91, 199, 191, 255, 1, 107, 221, 20, 221, 60, 164, 213, 111, 173, 177, 174, 213, 11, 118, 58, 146, 206, 65, 38, 76, 85, 88, 33, 222, 41, 239, 146, 134, 107, 194, 221, 39, 128, 165, 107, 241, 178 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 26, 13, 31, 46, 959, DateTimeKind.Utc).AddTicks(8966), new byte[] { 74, 49, 112, 93, 42, 72, 125, 104, 90, 3, 213, 70, 150, 21, 158, 112, 183, 229, 15, 187, 241, 89, 42, 14, 249, 230, 23, 183, 14, 92, 47, 85, 175, 105, 115, 108, 75, 174, 187, 122, 177, 221, 253, 160, 85, 206, 170, 9, 224, 17, 250, 104, 134, 208, 171, 128, 8, 238, 49, 125, 161, 160, 104, 68 }, new byte[] { 196, 19, 145, 159, 205, 210, 73, 90, 103, 191, 137, 191, 68, 69, 149, 31, 120, 45, 25, 222, 10, 75, 110, 106, 12, 117, 0, 239, 9, 18, 253, 237, 15, 237, 38, 52, 174, 108, 221, 180, 2, 76, 157, 101, 8, 85, 19, 143, 186, 139, 136, 36, 250, 211, 37, 131, 110, 240, 55, 117, 250, 43, 29, 242, 39, 71, 74, 121, 139, 240, 130, 227, 14, 72, 159, 124, 68, 55, 11, 151, 250, 229, 232, 26, 8, 253, 42, 161, 218, 52, 120, 91, 194, 97, 91, 131, 189, 2, 13, 52, 101, 89, 36, 45, 253, 106, 98, 135, 157, 23, 124, 36, 137, 223, 153, 213, 12, 253, 31, 178, 45, 110, 21, 25, 62, 137, 72, 178 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 26, 13, 31, 46, 959, DateTimeKind.Utc).AddTicks(8968), new byte[] { 59, 223, 226, 179, 45, 137, 12, 134, 216, 221, 186, 5, 45, 18, 214, 172, 255, 138, 245, 152, 118, 87, 171, 60, 205, 150, 177, 86, 90, 253, 52, 118, 60, 192, 35, 155, 175, 15, 202, 241, 53, 240, 193, 33, 125, 103, 151, 101, 61, 19, 132, 139, 75, 58, 63, 6, 82, 68, 187, 7, 249, 187, 123, 192 }, new byte[] { 199, 242, 251, 100, 33, 184, 123, 127, 208, 233, 217, 147, 139, 102, 1, 40, 196, 209, 90, 8, 254, 113, 102, 59, 84, 55, 51, 1, 171, 101, 78, 180, 54, 5, 222, 137, 219, 251, 177, 88, 189, 41, 100, 163, 180, 191, 56, 39, 226, 195, 42, 105, 152, 63, 236, 90, 127, 23, 139, 135, 58, 31, 210, 190, 190, 170, 81, 83, 19, 154, 118, 127, 232, 88, 183, 78, 204, 70, 61, 197, 247, 142, 11, 4, 26, 145, 77, 251, 151, 129, 242, 162, 131, 29, 245, 58, 6, 221, 40, 231, 237, 120, 12, 181, 26, 44, 104, 142, 140, 24, 35, 187, 39, 238, 68, 2, 194, 1, 109, 179, 251, 70, 119, 141, 101, 192, 200, 156 } });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswerOptions_QuestionSubmissionId1",
                table: "SubmissionAnswerOptions",
                column: "QuestionSubmissionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionAnswerOptions_QuestionSubmissions_QuestionSubmis~1",
                table: "SubmissionAnswerOptions",
                column: "QuestionSubmissionId1",
                principalTable: "QuestionSubmissions",
                principalColumn: "QuestionSubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionAnswerOptions_QuestionSubmissions_QuestionSubmis~1",
                table: "SubmissionAnswerOptions");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionAnswerOptions_QuestionSubmissionId1",
                table: "SubmissionAnswerOptions");

            migrationBuilder.DropColumn(
                name: "QuestionSubmissionId1",
                table: "SubmissionAnswerOptions");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "QuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "GradedAttempts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 24, 10, 38, 30, 134, DateTimeKind.Utc).AddTicks(3353), new byte[] { 7, 85, 7, 161, 35, 199, 22, 228, 75, 128, 44, 102, 6, 104, 122, 254, 1, 80, 138, 27, 58, 243, 179, 127, 123, 162, 29, 46, 104, 178, 159, 93, 247, 69, 73, 105, 13, 102, 127, 208, 32, 58, 201, 139, 87, 250, 154, 143, 90, 127, 31, 74, 38, 154, 81, 45, 112, 130, 220, 122, 59, 38, 19, 173 }, new byte[] { 155, 179, 51, 73, 203, 44, 238, 47, 147, 117, 128, 75, 101, 72, 38, 42, 63, 167, 103, 116, 134, 202, 241, 167, 56, 153, 114, 71, 160, 81, 110, 14, 80, 98, 151, 116, 233, 112, 197, 169, 84, 118, 245, 186, 140, 64, 200, 143, 232, 115, 121, 125, 171, 19, 164, 73, 91, 220, 178, 111, 194, 79, 192, 90, 45, 150, 114, 128, 128, 194, 74, 46, 77, 95, 15, 72, 139, 87, 94, 42, 241, 234, 223, 172, 171, 23, 91, 49, 235, 251, 158, 162, 178, 51, 216, 99, 216, 72, 203, 243, 243, 108, 135, 189, 131, 17, 147, 141, 68, 72, 77, 249, 191, 100, 187, 64, 36, 159, 21, 141, 48, 93, 234, 223, 211, 202, 2, 174 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 24, 10, 38, 30, 134, DateTimeKind.Utc).AddTicks(3360), new byte[] { 17, 155, 239, 21, 242, 217, 24, 12, 220, 129, 99, 191, 78, 123, 59, 120, 184, 15, 227, 76, 20, 139, 197, 201, 88, 227, 120, 60, 105, 190, 79, 4, 12, 204, 117, 156, 217, 114, 105, 86, 64, 75, 22, 60, 151, 143, 118, 98, 93, 177, 116, 24, 232, 166, 229, 86, 17, 163, 83, 74, 92, 159, 121, 194 }, new byte[] { 35, 109, 80, 189, 214, 150, 245, 189, 255, 211, 156, 105, 236, 89, 218, 83, 135, 48, 238, 117, 1, 66, 177, 98, 203, 242, 128, 204, 14, 233, 88, 255, 163, 76, 240, 199, 242, 199, 246, 35, 207, 149, 1, 229, 195, 149, 221, 10, 113, 48, 175, 167, 91, 218, 144, 25, 125, 180, 12, 133, 175, 210, 83, 174, 106, 16, 154, 107, 20, 97, 131, 154, 151, 196, 32, 103, 141, 23, 71, 139, 119, 161, 166, 217, 21, 168, 109, 63, 138, 30, 0, 231, 152, 172, 76, 213, 215, 145, 95, 212, 42, 192, 211, 13, 186, 80, 228, 49, 185, 183, 61, 199, 194, 195, 178, 209, 22, 151, 130, 198, 22, 178, 239, 214, 62, 192, 253, 169 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 24, 10, 38, 30, 134, DateTimeKind.Utc).AddTicks(3362), new byte[] { 112, 213, 143, 217, 132, 108, 8, 93, 176, 91, 73, 253, 25, 14, 24, 215, 8, 218, 176, 243, 31, 15, 208, 212, 212, 191, 162, 161, 218, 131, 15, 244, 103, 71, 12, 128, 81, 28, 116, 71, 237, 201, 233, 184, 153, 146, 136, 41, 74, 247, 21, 60, 150, 46, 223, 102, 213, 8, 137, 24, 172, 138, 179, 252 }, new byte[] { 230, 30, 142, 155, 15, 215, 60, 170, 138, 159, 252, 161, 198, 170, 33, 69, 49, 5, 179, 16, 29, 132, 105, 48, 167, 89, 140, 91, 64, 233, 129, 114, 194, 20, 227, 118, 130, 211, 121, 114, 183, 221, 74, 216, 104, 99, 220, 218, 121, 125, 166, 208, 213, 84, 155, 147, 27, 80, 194, 147, 207, 18, 188, 164, 247, 88, 83, 50, 24, 181, 200, 14, 106, 155, 148, 129, 42, 60, 194, 254, 27, 104, 240, 134, 169, 62, 72, 75, 112, 127, 45, 239, 150, 87, 84, 34, 14, 102, 243, 189, 173, 160, 132, 139, 17, 123, 88, 219, 114, 221, 44, 234, 105, 29, 83, 103, 57, 186, 135, 23, 146, 44, 173, 77, 46, 1, 29, 135 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 1, 24, 10, 38, 30, 134, DateTimeKind.Utc).AddTicks(3364), new byte[] { 32, 203, 47, 221, 59, 130, 102, 33, 217, 168, 222, 240, 247, 204, 251, 5, 2, 141, 100, 109, 17, 179, 112, 94, 24, 73, 196, 228, 159, 147, 29, 78, 218, 148, 244, 8, 76, 158, 62, 151, 57, 223, 80, 150, 209, 175, 43, 225, 147, 68, 137, 116, 76, 29, 30, 74, 144, 116, 219, 244, 64, 62, 29, 217 }, new byte[] { 38, 161, 222, 130, 21, 216, 158, 81, 123, 165, 29, 39, 219, 215, 172, 170, 155, 204, 104, 40, 239, 235, 124, 105, 174, 28, 137, 88, 112, 208, 239, 130, 215, 181, 29, 186, 1, 207, 215, 126, 230, 105, 72, 43, 123, 30, 55, 230, 140, 23, 186, 120, 143, 191, 186, 191, 71, 107, 211, 135, 118, 126, 125, 117, 49, 239, 103, 48, 210, 12, 125, 75, 144, 9, 236, 191, 94, 129, 208, 33, 177, 136, 121, 103, 149, 248, 81, 135, 41, 158, 221, 91, 104, 206, 194, 50, 165, 173, 154, 229, 1, 211, 164, 156, 136, 67, 175, 253, 151, 46, 31, 97, 217, 35, 15, 244, 29, 236, 47, 144, 62, 10, 107, 19, 59, 22, 142, 218 } });
        }
    }
}

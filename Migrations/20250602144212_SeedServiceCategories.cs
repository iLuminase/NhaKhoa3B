using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedServiceCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed ServiceCategories
            migrationBuilder.Sql("SET IDENTITY_INSERT ServiceCategories ON;");

            migrationBuilder.Sql(@"
                INSERT INTO ServiceCategories (Id, Name, Description, IsActive, CreatedAt) VALUES
                (1, N'Điều trị cơ bản', N'Các dịch vụ điều trị nha khoa cơ bản', 1, GETDATE()),
                (2, N'Phục hình răng', N'Các dịch vụ phục hình và thẩm mỹ răng', 1, GETDATE()),
                (3, N'Nha chu', N'Điều trị các bệnh về nha chu', 1, GETDATE()),
                (4, N'Nội nha', N'Điều trị tủy răng và các bệnh lý trong răng', 1, GETDATE()),
                (5, N'Phẫu thuật', N'Các phẫu thuật nha khoa', 1, GETDATE()),
                (6, N'Niềng răng', N'Chỉnh nha và niềng răng', 1, GETDATE()),
                (7, N'Vệ sinh răng miệng', N'Dịch vụ vệ sinh và phòng ngừa', 1, GETDATE());
            ");

            migrationBuilder.Sql("SET IDENTITY_INSERT ServiceCategories OFF;");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cbf5694-d2f2-469e-a26e-1a09d6a447f2", new DateTime(2025, 6, 2, 21, 42, 11, 809, DateTimeKind.Local).AddTicks(8186), "AQAAAAIAAYagAAAAEEbIPCKCfnX6WQE0+rYkfi8Om5ZBmxj0Zdd0R/226FcH0oCikG0lrCZtBw8F0Lvvaw==", "dcc7d899-bc4d-437d-9a24-26b35f6b5d14" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "116727fa-961c-4ff2-a069-c4e2b2b635fb", new DateTime(2025, 6, 2, 21, 41, 42, 49, DateTimeKind.Local).AddTicks(2788), "AQAAAAIAAYagAAAAEKvd+SMi77gKk7ym3i9Cp1LF0X1VKOIMeY9ZZWhuWC1X2s1dP8WaTaco8oVinQpmYw==", "1c99fef7-3c36-4c3f-a688-680f2fdc6898" });
        }
    }
}

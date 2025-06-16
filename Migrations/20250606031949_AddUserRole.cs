using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ApplicationUserId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4", null, null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "245fe53a-7f94-45bb-a133-82f12de31bf2", new DateTime(2025, 6, 6, 10, 19, 47, 889, DateTimeKind.Local).AddTicks(9744), "AQAAAAIAAYagAAAAEJC8adRI1s8FPbiYesNqGOv5jHaodr3Fp22qzKWeIQUJ6OACTtt6O+gDpF9svaJzfg==", "dd876d6d-66e0-4a1b-abf8-2f247fd04c81" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cbf5694-d2f2-469e-a26e-1a09d6a447f2", new DateTime(2025, 6, 2, 21, 42, 11, 809, DateTimeKind.Local).AddTicks(8186), "AQAAAAIAAYagAAAAEEbIPCKCfnX6WQE0+rYkfi8Om5ZBmxj0Zdd0R/226FcH0oCikG0lrCZtBw8F0Lvvaw==", "dcc7d899-bc4d-437d-9a24-26b35f6b5d14" });
        }
    }
}

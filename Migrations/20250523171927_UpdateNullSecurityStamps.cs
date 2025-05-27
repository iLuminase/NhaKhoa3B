using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNullSecurityStamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Patients_PatientId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PatientId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Payments");

            // Update all users with null SecurityStamp
            migrationBuilder.Sql(@"
                UPDATE Users
                SET SecurityStamp = NEWID()
                WHERE SecurityStamp IS NULL OR SecurityStamp = ''
            ");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0059427d-ea9a-4fd5-9d87-d8f7d9e0767f", new DateTime(2025, 5, 24, 0, 19, 26, 858, DateTimeKind.Local).AddTicks(5767), "AQAAAAIAAYagAAAAEJCxGOOD7R29jtcHZKXtxtP6aAmRT4TMM53GdI9PXaFD4QfDNAcJFoRpk0RmZjDfCQ==", "b5c9f7e6-40f8-41be-936b-c75599cd7fa8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3c2ae855-6c03-4400-a5b6-f8cfc729b671", new DateTime(2025, 5, 23, 23, 4, 33, 425, DateTimeKind.Local).AddTicks(3369), "AQAAAAIAAYagAAAAEHDqPUFbV+dNaL1xS8qyz8CRZffun/OzKDRkuuT4KtrySXcYj8HjvwZqTAUlo2AFMg==", "6f698628-f3e8-480e-a7da-896c9dc0716f" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PatientId1",
                table: "Payments",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Patients_PatientId1",
                table: "Payments",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}

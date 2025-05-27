using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoMoTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a76b991e-a358-4fb9-a5df-d6dbfaff7d56", new DateTime(2025, 5, 24, 11, 15, 24, 305, DateTimeKind.Local).AddTicks(7580), "AQAAAAIAAYagAAAAELqcx2VPxjMleLbDmOZDVK90ss3hjob3sQSBSYzzt0ZiZ7atWS0kR12c9fM3y379Nw==", "4779b1f4-0cda-430a-bc9a-3f737cd0362e" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_AppointmentId",
                table: "PaymentTransactions",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0059427d-ea9a-4fd5-9d87-d8f7d9e0767f", new DateTime(2025, 5, 24, 0, 19, 26, 858, DateTimeKind.Local).AddTicks(5767), "AQAAAAIAAYagAAAAEJCxGOOD7R29jtcHZKXtxtP6aAmRT4TMM53GdI9PXaFD4QfDNAcJFoRpk0RmZjDfCQ==", "b5c9f7e6-40f8-41be-936b-c75599cd7fa8" });
        }
    }
}

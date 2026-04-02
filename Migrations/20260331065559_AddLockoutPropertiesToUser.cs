using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLockoutPropertiesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 1, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6779) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 1, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6783) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 2, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6785) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 2, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6788) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 3, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6790) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 3, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6793) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 4, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6795) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 5, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6797) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 5, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6799) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6746), new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6802) });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6712));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6715));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6726));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6729));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6731));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6638));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6640));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6643));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6645));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6647));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6648));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6655));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 13, 55, 58, 712, DateTimeKind.Local).AddTicks(6657));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "849250c5-e824-4306-a6db-04f45f5e7846", new DateTime(2026, 3, 31, 13, 55, 58, 229, DateTimeKind.Local).AddTicks(1874), false, null, "AQAAAAIAAYagAAAAECzYccRlFUkXVcfnhtyEP4pva2gYk8iYKFKX2fMiYEmr9mwSCW9tBnMEzNSsSjw1IQ==", "97be91f1-fe92-47fd-9bf6-ae1c200214ac" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7d0f843f-0800-4656-974a-dbf36734329c", new DateTime(2026, 3, 31, 13, 55, 58, 292, DateTimeKind.Local).AddTicks(3673), false, null, "AQAAAAIAAYagAAAAEGAu406J9QH4m04QCx8lsXfzroywNX+mB0/5WZ1GmBZRZk/uXP07BJrud0K06VCtnQ==", "271cdff1-dc7d-4184-b4ec-bf06a94f6f73" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b72484b-7f18-40e7-b7af-50a4e1e6a79f", new DateTime(2026, 3, 31, 13, 55, 58, 353, DateTimeKind.Local).AddTicks(4938), false, null, "AQAAAAIAAYagAAAAEJMAyYb+9Ak10iN/npbYv9l1JnaOZXXy7K5ggU4p7KUbV0kWrMgZVqI9UGNEu03lwg==", "ea644be8-82e9-4180-81a8-98a24b833654" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e3c41b9-00ea-4884-8bef-2fef3a9d2323", new DateTime(2026, 3, 31, 13, 55, 58, 414, DateTimeKind.Local).AddTicks(3947), false, null, "AQAAAAIAAYagAAAAEE+EMCko1wH6/FY2WD2gmhU3ysEQNqmzMLKVXJPeHfvzJ3lA5Hjm1L2sgk5axS9T7Q==", "6847374b-b24d-4450-bc90-853d71b10cb2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2bdd49b4-0776-442d-9cf6-22c9d5415a51", new DateTime(2026, 3, 31, 13, 55, 58, 473, DateTimeKind.Local).AddTicks(9188), false, null, "AQAAAAIAAYagAAAAEGS/LCLYE57e0vW65SlbRu6dDQcKHRh+LubEMijxQTR+qh/Kff03RFVDPoOh+aJlng==", "8c30e0b9-4d42-44c9-9356-3b4e54476d4c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc85c5ad-0e6c-40f0-8f6b-45171d6a52de", new DateTime(2026, 3, 31, 13, 55, 58, 533, DateTimeKind.Local).AddTicks(4336), false, null, "AQAAAAIAAYagAAAAEO1de6htRJEUI4Rb4hSttJCHmVU0JJB/BuliQxsiQe+fTgiUBqob6xfKWJAKXOGA2A==", "a0370dc1-813f-4e9e-9439-4b1cab3f0b5c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7718ab2d-f18b-4dcc-888d-352303046068", new DateTime(2026, 3, 31, 13, 55, 58, 593, DateTimeKind.Local).AddTicks(3243), false, null, "AQAAAAIAAYagAAAAEJEAaaPbE2G0T4EPBBSQ4ke9Qx+PmWOXbq8QPyQ1T7r1R+DOdsxHqgg7Ls/nybZe5w==", "5ca4425d-1967-41e5-950f-1a6eddb64382" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "IsLocked", "LockoutTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09f4be8b-7dd1-40e0-9287-7aa1edc72aed", new DateTime(2026, 3, 31, 13, 55, 58, 652, DateTimeKind.Local).AddTicks(5889), false, null, "AQAAAAIAAYagAAAAENZH8r/C+vHqJFq99r8exXw9eRonncoofwlUGasfbkXmBoPExQVNmTdFeB3Bsu85rw==", "edd9ee19-683a-4133-9c19-b1bc1f8142ff" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutTime",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1473) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1476) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 1, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1479) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 1, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1482) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 2, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1484) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 2, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1486) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 3, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1489) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 4, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1493) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 4, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1496) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 5, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1498) });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1402));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1405));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1408));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1417));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1419));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1422));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1425));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1427));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1327));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1329));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1332));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1334));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1336));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1338));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1342));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1344));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e75ce2f-6f53-4402-b955-9fbe6c3678e4", new DateTime(2026, 3, 30, 17, 55, 23, 830, DateTimeKind.Local).AddTicks(3555), "AQAAAAIAAYagAAAAEBFJ7s0dkRl028F8JbAIIWdLnMpsFZXNZu7WzFymlEM9CuDbRQJdkssTcHzEXvy5Dg==", "29a6b1cf-aea8-4768-b2a1-25b0cdb2d42b" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26b0fc42-5498-46fe-83d7-95266defb013", new DateTime(2026, 3, 30, 17, 55, 23, 891, DateTimeKind.Local).AddTicks(2269), "AQAAAAIAAYagAAAAEBVDpIZD+qW6iNblfHAVt6/3v8rNK+MZb3Sa/7zLAmv7WCbzeMHJ/oEoL75DPFgpBQ==", "41c1e52b-c689-4585-9f24-982aa5488dfe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2680803-3bfc-425e-9446-27d7e8f04c61", new DateTime(2026, 3, 30, 17, 55, 23, 953, DateTimeKind.Local).AddTicks(475), "AQAAAAIAAYagAAAAEOynxcc36v90zg9yAP9jwgjTheZvdL8u8H0sSzK/z33rbax1UgApbhTJtU/wvKqPYw==", "2130117b-ff53-48aa-a0e0-6c157d930645" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5678ebcd-b450-4b41-9079-e3fdad0049fd", new DateTime(2026, 3, 30, 17, 55, 24, 14, DateTimeKind.Local).AddTicks(356), "AQAAAAIAAYagAAAAEGpn1rKbNl+uOFm89mzWy6Bs2OsiR9+gzY6MwOsXbxtLmzseFqjjJPfTKJkR2QF/Jw==", "35750c52-3a24-4604-85fa-c92a7df69747" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "91ac7091-f9b1-4cef-bcaa-43e7b4d35251", new DateTime(2026, 3, 30, 17, 55, 24, 74, DateTimeKind.Local).AddTicks(6953), "AQAAAAIAAYagAAAAENqAKaJ+C6c/R6uJomgENkl2DbsJcD33XfiUxhiqhgJq9A8pLDkEoNlLevSQWtOMlg==", "aed411a5-e922-49d2-8254-c9e40e77aeb4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9b8a5006-dad2-491d-82a9-983bb42cd9a4", new DateTime(2026, 3, 30, 17, 55, 24, 135, DateTimeKind.Local).AddTicks(2615), "AQAAAAIAAYagAAAAENFan8CLr0OEm040eiQW8sTXf+iLDp0w6qQcRGPMxf43xXfDiIezSLJ+CvoDAsw5Xw==", "89ab51c2-f58e-4efb-ac43-b6874cbad826" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e07a9e80-2914-4f97-b868-84ade49d4071", new DateTime(2026, 3, 30, 17, 55, 24, 199, DateTimeKind.Local).AddTicks(1864), "AQAAAAIAAYagAAAAEOI/q44is/xJ3QJ7LnLzS71rPPw+YY0G9e8WDwo7vucZXKNzhIE81uvkNMBW1pFXJA==", "13b3fb97-1ef5-49e1-a58f-02f43c020a44" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43e4ecb6-01df-4695-b0a6-508346df6674", new DateTime(2026, 3, 30, 17, 55, 24, 260, DateTimeKind.Local).AddTicks(7291), "AQAAAAIAAYagAAAAELMIe00GWHfMEgOUvKA5n1+TcCl9rAKXUuoMBFg5x4TO3s0gqxP41W2Rga51XrMChg==", "239f0114-e496-4229-a897-6383c662bf0b" });
        }
    }
}

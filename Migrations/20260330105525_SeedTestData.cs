using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "Allergies", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IsActive", "MedicalHistory", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Đường ABC, Quận 1, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1402), new DateOnly(1990, 1, 15), "an.nguyen@example.com", "Nguyễn Văn An", "Nam", true, null, "0901234567", null },
                    { 2, "456 Đường XYZ, Quận 2, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1405), new DateOnly(1992, 5, 20), "bich.tran@example.com", "Trần Thị Bích", "Nữ", true, null, "0902345678", null },
                    { 3, "789 Đường MNO, Quận 3, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1408), new DateOnly(1988, 3, 10), "cong.le@example.com", "Lê Văn Công", "Nam", true, null, "0903456789", null },
                    { 4, "321 Đường PQR, Quận 4, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1410), new DateOnly(1995, 7, 25), "dung.pham@example.com", "Phạm Thị Dung", "Nữ", true, null, "0904567890", null },
                    { 5, "654 Đường STU, Quận 5, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1412), new DateOnly(1991, 9, 8), "em.hoang@example.com", "Hoàng Văn Em", "Nam", true, null, "0905678901", null },
                    { 6, "987 Đường VWX, Quận 6, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1417), new DateOnly(1993, 11, 12), "huong.vu@example.com", "Vũ Thị Hương", "Nữ", true, null, "0906789012", null },
                    { 7, "147 Đường YZA, Quận 7, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1419), new DateOnly(1989, 2, 18), "ich.dang@example.com", "Đặng Văn Ích", "Nam", true, null, "0907890123", null },
                    { 8, "258 Đường BCD, Quận 8, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1422), new DateOnly(1996, 4, 30), "khanh.ngo@example.com", "Ngô Thị Khánh", "Nữ", true, null, "0908901234", null },
                    { 9, "369 Đường EFG, Quận 9, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1425), new DateOnly(1987, 6, 5), "lam.ta@example.com", "Tạ Văn Lâm", "Nam", true, null, "0909012345", null },
                    { 10, "741 Đường HIJ, Quận 10, TP.HCM", null, new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1427), new DateOnly(1994, 8, 22), "mong.duong@example.com", "Dương Thị Mộng", "Nữ", true, null, "0900123456", null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Category", "CreatedAt", "CreatedBy", "Description", "DurationMinutes", "IsActive", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Khám", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1327), "1", "Kiểm tra toàn bộ răng và nướu", 30, true, "Khám nha", 150000m, null },
                    { 2, "Làm sạch", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1329), "1", "Lấy cao răng và vệ sinh toàn bộ", 45, true, "Vệ sinh răng", 300000m, null },
                    { 3, "Điều trị", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1332), "1", "Trám sâu răng với vật liệu composite", 45, true, "Trám răng", 200000m, null },
                    { 4, "Điều trị", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1334), "1", "Nhổ răng hư hỏng nặng", 60, true, "Nhổ răng", 250000m, null },
                    { 5, "Niềng răng", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1336), "1", "Niềng răng để chỉnh sửa hàm", 120, true, "Niềng răng", 5000000m, null },
                    { 6, "Làm sạch", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1338), "1", "Tẩy trắng an toàn cho răng", 60, true, "Tẩy trắng răng", 400000m, null },
                    { 7, "Trồng implant", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1340), "1", "Cấy ghép implant để thay thế răng mất", 180, true, "Nhân tạo implant", 10000000m, null },
                    { 8, "Làm sạch", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1342), "1", "Tiêm botox chuyên biệt cho nướu", 30, true, "Tiêm botox nướu", 350000m, null },
                    { 9, "Điều trị", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1344), "1", "Điều trị viêm nướu và chảy máu", 45, true, "Xử lý viêm nướu", 250000m, null },
                    { 10, "Điều trị", new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1346), "1", "Bọc sứ để phục hình răng hỏng", 90, true, "Bọc sứ răng", 800000m, null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e75ce2f-6f53-4402-b955-9fbe6c3678e4", new DateTime(2026, 3, 30, 17, 55, 23, 830, DateTimeKind.Local).AddTicks(3555), "AQAAAAIAAYagAAAAEBFJ7s0dkRl028F8JbAIIWdLnMpsFZXNZu7WzFymlEM9CuDbRQJdkssTcHzEXvy5Dg==", "29a6b1cf-aea8-4768-b2a1-25b0cdb2d42b" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2", 0, null, "26b0fc42-5498-46fe-83d7-95266defb013", new DateTime(2026, 3, 30, 17, 55, 23, 891, DateTimeKind.Local).AddTicks(2269), new DateOnly(1985, 5, 15), "dentist1@dental.com", true, "Dr. Nguyễn Văn A", "Male", true, false, null, "DENTIST1@DENTAL.COM", "DENTIST1@DENTAL.COM", "AQAAAAIAAYagAAAAEBVDpIZD+qW6iNblfHAVt6/3v8rNK+MZb3Sa/7zLAmv7WCbzeMHJ/oEoL75DPFgpBQ==", null, false, "41c1e52b-c689-4585-9f24-982aa5488dfe", false, "dentist1@dental.com" },
                    { "3", 0, null, "b2680803-3bfc-425e-9446-27d7e8f04c61", new DateTime(2026, 3, 30, 17, 55, 23, 953, DateTimeKind.Local).AddTicks(475), new DateOnly(1987, 3, 20), "dentist2@dental.com", true, "Dr. Trần Thị B", "Female", true, false, null, "DENTIST2@DENTAL.COM", "DENTIST2@DENTAL.COM", "AQAAAAIAAYagAAAAEOynxcc36v90zg9yAP9jwgjTheZvdL8u8H0sSzK/z33rbax1UgApbhTJtU/wvKqPYw==", null, false, "2130117b-ff53-48aa-a0e0-6c157d930645", false, "dentist2@dental.com" },
                    { "4", 0, null, "5678ebcd-b450-4b41-9079-e3fdad0049fd", new DateTime(2026, 3, 30, 17, 55, 24, 14, DateTimeKind.Local).AddTicks(356), new DateOnly(1989, 7, 10), "dentist3@dental.com", true, "Dr. Lê Văn C", "Male", true, false, null, "DENTIST3@DENTAL.COM", "DENTIST3@DENTAL.COM", "AQAAAAIAAYagAAAAEGpn1rKbNl+uOFm89mzWy6Bs2OsiR9+gzY6MwOsXbxtLmzseFqjjJPfTKJkR2QF/Jw==", null, false, "35750c52-3a24-4604-85fa-c92a7df69747", false, "dentist3@dental.com" },
                    { "5", 0, null, "91ac7091-f9b1-4cef-bcaa-43e7b4d35251", new DateTime(2026, 3, 30, 17, 55, 24, 74, DateTimeKind.Local).AddTicks(6953), new DateOnly(1991, 9, 25), "dentist4@dental.com", true, "Dr. Phạm Thị D", "Female", true, false, null, "DENTIST4@DENTAL.COM", "DENTIST4@DENTAL.COM", "AQAAAAIAAYagAAAAENqAKaJ+C6c/R6uJomgENkl2DbsJcD33XfiUxhiqhgJq9A8pLDkEoNlLevSQWtOMlg==", null, false, "aed411a5-e922-49d2-8254-c9e40e77aeb4", false, "dentist4@dental.com" },
                    { "6", 0, null, "9b8a5006-dad2-491d-82a9-983bb42cd9a4", new DateTime(2026, 3, 30, 17, 55, 24, 135, DateTimeKind.Local).AddTicks(2615), new DateOnly(1993, 2, 14), "staff1@dental.com", true, "Nguyễn Thị E", "Female", true, false, null, "STAFF1@DENTAL.COM", "STAFF1@DENTAL.COM", "AQAAAAIAAYagAAAAENFan8CLr0OEm040eiQW8sTXf+iLDp0w6qQcRGPMxf43xXfDiIezSLJ+CvoDAsw5Xw==", null, false, "89ab51c2-f58e-4efb-ac43-b6874cbad826", false, "staff1@dental.com" },
                    { "7", 0, null, "e07a9e80-2914-4f97-b868-84ade49d4071", new DateTime(2026, 3, 30, 17, 55, 24, 199, DateTimeKind.Local).AddTicks(1864), new DateOnly(1994, 6, 8), "staff2@dental.com", true, "Trần Văn F", "Male", true, false, null, "STAFF2@DENTAL.COM", "STAFF2@DENTAL.COM", "AQAAAAIAAYagAAAAEOI/q44is/xJ3QJ7LnLzS71rPPw+YY0G9e8WDwo7vucZXKNzhIE81uvkNMBW1pFXJA==", null, false, "13b3fb97-1ef5-49e1-a58f-02f43c020a44", false, "staff2@dental.com" },
                    { "8", 0, null, "43e4ecb6-01df-4695-b0a6-508346df6674", new DateTime(2026, 3, 30, 17, 55, 24, 260, DateTimeKind.Local).AddTicks(7291), new DateOnly(1995, 11, 30), "staff3@dental.com", true, "Lê Thị G", "Female", true, false, null, "STAFF3@DENTAL.COM", "STAFF3@DENTAL.COM", "AQAAAAIAAYagAAAAELMIe00GWHfMEgOUvKA5n1+TcCl9rAKXUuoMBFg5x4TO3s0gqxP41W2Rga51XrMChg==", null, false, "239f0114-e496-4229-a897-6383c662bf0b", false, "staff3@dental.com" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "CreatedAt", "CreatedBy", "CreatedByUserId", "DentistId", "EndTime", "Notes", "PatientId", "PatientName", "ServiceId", "StartTime", "Status", "TreatmentType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 31, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1473), "System", "1", "2", new TimeSpan(0, 9, 30, 0, 0), null, 1, "Nguyễn Văn An", 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", null, null },
                    { 2, new DateTime(2026, 3, 31, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1476), "System", "1", "3", new TimeSpan(0, 10, 45, 0, 0), null, 2, "Trần Thị Bích", 2, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", null, null },
                    { 3, new DateTime(2026, 4, 1, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1479), "System", "1", "4", new TimeSpan(0, 11, 45, 0, 0), null, 3, "Lê Văn Công", 3, new TimeSpan(0, 11, 0, 0, 0), "Scheduled", null, null },
                    { 4, new DateTime(2026, 4, 1, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1482), "System", "1", "5", new TimeSpan(0, 15, 0, 0, 0), null, 4, "Phạm Thị Dung", 4, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", null, null },
                    { 5, new DateTime(2026, 4, 2, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1484), "System", "1", "2", new TimeSpan(0, 10, 30, 0, 0), null, 5, "Hoàng Văn Em", 5, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", null, null },
                    { 6, new DateTime(2026, 4, 2, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1486), "System", "1", "3", new TimeSpan(0, 16, 0, 0, 0), null, 6, "Vũ Thị Hương", 6, new TimeSpan(0, 15, 0, 0, 0), "Completed", null, null },
                    { 7, new DateTime(2026, 4, 3, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1489), "System", "1", "4", new TimeSpan(0, 12, 0, 0, 0), null, 7, "Đặng Văn Ích", 7, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", null, null },
                    { 8, new DateTime(2026, 4, 4, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1493), "System", "1", "5", new TimeSpan(0, 13, 30, 0, 0), null, 8, "Ngô Thị Khánh", 8, new TimeSpan(0, 13, 0, 0, 0), "Scheduled", null, null },
                    { 9, new DateTime(2026, 4, 4, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1496), "System", "1", "2", new TimeSpan(0, 16, 45, 0, 0), null, 9, "Tạ Văn Lâm", 9, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", null, null },
                    { 10, new DateTime(2026, 4, 5, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1443), new DateTime(2026, 3, 30, 17, 55, 24, 326, DateTimeKind.Local).AddTicks(1498), "System", "1", "3", new TimeSpan(0, 12, 30, 0, 0), null, 10, "Dương Thị Mộng", 10, new TimeSpan(0, 11, 0, 0, 0), "Scheduled", null, null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "2" },
                    { "2", "3" },
                    { "2", "4" },
                    { "2", "5" },
                    { "3", "6" },
                    { "3", "7" },
                    { "3", "8" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "3" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "4" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "5" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "6" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "7" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "8" });

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0938f911-7228-4c7c-9d8e-844b662bafe8", new DateTime(2026, 3, 30, 17, 26, 12, 83, DateTimeKind.Local).AddTicks(9450), "AQAAAAIAAYagAAAAEM4YixjEM0QZKfrWolecmFGOipYr5HNV5LfcgspkWUrhdHZ9qp8kCh6gZLvMXGPU2A==", "ede809b8-01ce-4df2-9bcd-5e17b6b89e4b" });
        }
    }
}

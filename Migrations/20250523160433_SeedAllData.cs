using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3c2ae855-6c03-4400-a5b6-f8cfc729b671", new DateTime(2025, 5, 23, 23, 4, 33, 425, DateTimeKind.Local).AddTicks(3369), "AQAAAAIAAYagAAAAEHDqPUFbV+dNaL1xS8qyz8CRZffun/OzKDRkuuT4KtrySXcYj8HjvwZqTAUlo2AFMg==", "6f698628-f3e8-480e-a7da-896c9dc0716f" });

            // Seed Dentist User
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = N'dentist1')
            BEGIN
                INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FullName, DateOfBirth, Gender, Address, CreatedAt, IsActive)
                VALUES (
                    N'dentist1', N'dentist@dental.com', N'DENTIST@DENTAL.COM', N'dentist@dental.com', N'DENTIST@DENTAL.COM', 1,
                    N'AQAAAAIAAYagAAAAEOqg/fYrCbHWeUKeGcXMHA4zsSukurpZqzOOtiopfEtmqddfpOscAiRNuK2+C99wAg==', 
                    NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0, N'Dr. Emily White', '1985-07-15', N'Nữ', N'123 Dental St, City', GETDATE(), 1
                );
            END
            ");
            // Note: You'll need to generate a proper password hash for 'dentist1'. The one above is a placeholder.
            // You can generate it by creating the user through your application UI once and copying the hash,
            // or use UserManager in a temporary script. For now, this user might not be able to log in with the placeholder hash.

            // Assign 'Dentist' role to 'dentist1' user (Role Id '2' was seeded in ApplicationDbContext)
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = N'dentist1' AND RoleId = N'2')
            BEGIN
                INSERT INTO UserRoles (UserId, RoleId) VALUES (N'dentist1', N'2');
            END
            ");

            // Seed Dentist User 2
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = N'dentist2')
            BEGIN
                INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FullName, DateOfBirth, Gender, Address, CreatedAt, IsActive)
                VALUES (
                    N'dentist2', N'dentist2@dental.com', N'DENTIST2@DENTAL.COM', N'dentist2@dental.com', N'DENTIST2@DENTAL.COM', 1,
                    N'AQAAAAIAAYagAAAAEKXEaXDjCxWR1iuuBLMreBmWN8hfMu2MFJzefXHapYjIJn9cBttFyXa1IvqPJvXaAg==', /* Hash for Dentist@234 */
                    NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0, N'Dr. John Smith', '1980-01-01', N'Nam', N'456 Oak St, City', GETDATE(), 1
                );
            END
            ");

            // Assign 'Dentist' role to 'dentist2' user
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = N'dentist2' AND RoleId = N'2')
            BEGIN
                INSERT INTO UserRoles (UserId, RoleId) VALUES (N'dentist2', N'2');
            END
            ");

            // Seed Staff User 1
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = N'staff1')
            BEGIN
                INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FullName, DateOfBirth, Gender, Address, CreatedAt, IsActive)
                VALUES (
                    N'staff1', N'staff1@dental.com', N'STAFF1@DENTAL.COM', N'staff1@dental.com', N'STAFF1@DENTAL.COM', 1,
                    N'AQAAAAIAAYagAAAAEIQeKS+fVEcC4LkN1k8XMMFOHkOxska+qj09vtkdOvyJAjWA6cii5PQHkow/gOQJdA==', /* Hash for Staff@123 */
                    NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0, N'Jane Doe', '1992-05-20', N'Nữ', N'789 Pine St, City', GETDATE(), 1
                );
            END
            ");

            // Assign 'Staff' role to 'staff1' user (Role Id '3' was seeded in ApplicationDbContext)
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = N'staff1' AND RoleId = N'3')
            BEGIN
                INSERT INTO UserRoles (UserId, RoleId) VALUES (N'staff1', N'3');
            END
            ");

            // Seed Patients
            migrationBuilder.Sql("SET IDENTITY_INSERT Patients ON;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Patients WHERE Id = 1) INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, CreatedAt, IsActive) VALUES (1, N'Nguyễn Văn An', N'an.nguyen@example.com', N'0901234567', N'123 Đường ABC, Quận 1, TP.HCM', '1990-01-15', N'Nam', GETDATE(), 1);");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Patients WHERE Id = 2) INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, CreatedAt, IsActive) VALUES (2, N'Trần Thị Bình', N'binh.tran@example.com', N'0902345678', N'456 Đường XYZ, Quận 2, TP.HCM', '1995-05-20', N'Nữ', GETDATE(), 1);");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Patients WHERE Id = 3) INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, CreatedAt, IsActive) VALUES (3, N'Lê Văn Cường', N'cuong.le@example.com', N'0903456789', N'789 Đường KLM, Quận 3, TP.HCM', '1985-10-30', N'Nam', GETDATE(), 1);");
            migrationBuilder.Sql("SET IDENTITY_INSERT Patients OFF;");

            // Seed Services
            migrationBuilder.Sql("SET IDENTITY_INSERT Services ON;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = 1) INSERT INTO Services (Id, Name, Description, Price, CreatedAt, Category, CreatedBy, Duration, IsActive) VALUES (1, N'Trám răng', N'Trám răng sâu, răng mẻ', 150000.00, '2025-05-19T16:26:46.000', N'Dịch vụ tổng quát', N'admin', '00:20:00', 1) ELSE UPDATE Services SET Name=N'Trám răng', Description=N'Trám răng sâu, răng mẻ', Price=150000.00, Category=N'Dịch vụ tổng quát', CreatedBy=N'admin', Duration='00:20:00', IsActive=1 WHERE Id = 1;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = 2) INSERT INTO Services (Id, Name, Description, Price, CreatedAt, Category, CreatedBy, Duration, IsActive) VALUES (2, N'Lấy cao răng', N'Làm sạch mảng bám và cao răng', 200000.00, GETDATE(), N'Dịch vụ tổng quát', N'admin', '00:30:00', 1) ELSE UPDATE Services SET Name=N'Lấy cao răng', Description=N'Làm sạch mảng bám và cao răng', Price=200000.00, Category=N'Dịch vụ tổng quát', CreatedBy=N'admin', Duration='00:30:00', IsActive=1 WHERE Id = 2;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = 3) INSERT INTO Services (Id, Name, Description, Price, CreatedAt, Category, CreatedBy, Duration, IsActive) VALUES (3, N'Nhổ răng khôn', N'Tiểu phẫu nhổ răng số 8', 1000000.00, GETDATE(), N'Tiểu phẫu', N'admin', '01:00:00', 1) ELSE UPDATE Services SET Name=N'Nhổ răng khôn', Description=N'Tiểu phẫu nhổ răng số 8', Price=1000000.00, Category=N'Tiểu phẫu', CreatedBy=N'admin', Duration='01:00:00', IsActive=1 WHERE Id = 3;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = 4) INSERT INTO Services (Id, Name, Description, Price, CreatedAt, Category, CreatedBy, Duration, IsActive) VALUES (4, N'Tẩy trắng răng', N'Làm trắng răng bằng công nghệ laser', 2500000.00, GETDATE(), N'Thẩm mỹ', N'admin', '01:30:00', 1) ELSE UPDATE Services SET Name=N'Tẩy trắng răng', Description=N'Làm trắng răng bằng công nghệ laser', Price=2500000.00, Category=N'Thẩm mỹ', CreatedBy=N'admin', Duration='01:30:00', IsActive=1 WHERE Id = 4;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = 5) INSERT INTO Services (Id, Name, Description, Price, CreatedAt, Category, CreatedBy, Duration, IsActive) VALUES (5, N'Niềng răng mắc cài', N'Chỉnh nha bằng mắc cài kim loại/sứ', 30000000.00, GETDATE(), N'Chỉnh nha', N'admin', '02:00:00', 1) ELSE UPDATE Services SET Name=N'Niềng răng mắc cài', Description=N'Chỉnh nha bằng mắc cài kim loại/sứ', Price=30000000.00, Category=N'Chỉnh nha', CreatedBy=N'admin', Duration='02:00:00', IsActive=1 WHERE Id = 5;");
            migrationBuilder.Sql("SET IDENTITY_INSERT Services OFF;");

            // Seed Appointments
            migrationBuilder.Sql("SET IDENTITY_INSERT Appointments ON;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Appointments WHERE Id = 24) INSERT INTO Appointments (Id, PatientId, DentistId, ServiceId, AppointmentDate, StartTime, EndTime, Status, Notes, CreatedAt, CreatedByUserId) VALUES (24, 2, N'dentist1', 2, '2025-05-26', '09:00:00', '09:30:00', N'Scheduled', N'Kiểm tra răng tổng quát và lấy cao răng', GETDATE(), N'1');");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Appointments WHERE Id = 25) INSERT INTO Appointments (Id, PatientId, DentistId, ServiceId, AppointmentDate, StartTime, EndTime, Status, Notes, CreatedAt, CreatedByUserId) VALUES (25, 3, N'dentist1', 3, '2025-05-27', '10:00:00', '11:00:00', N'Scheduled', N'Tư vấn nhổ răng khôn', GETDATE(), N'1');");
            migrationBuilder.Sql("SET IDENTITY_INSERT Appointments OFF;");

            // Seed DentalRecords
            migrationBuilder.Sql("SET IDENTITY_INSERT DentalRecords ON;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM DentalRecords WHERE Id = 1) INSERT INTO DentalRecords (Id, PatientId, DentistId, AppointmentId, RecordDate, Diagnosis, Treatment, Cost, CreatedAt, Notes) VALUES (1, 2, N'dentist1', 24, '2025-05-26', N'Nhiều cao răng, viêm nướu nhẹ', N'Lấy cao răng toàn hàm, hướng dẫn vệ sinh răng miệng', 200000.00, GETDATE(), N'Bệnh nhân hơi ê buốt');");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM DentalRecords WHERE Id = 2) INSERT INTO DentalRecords (Id, PatientId, DentistId, AppointmentId, RecordDate, Diagnosis, Treatment, Cost, CreatedAt, Notes) VALUES (2, 3, N'dentist1', 25, '2025-05-27', N'Răng khôn hàm dưới trái mọc lệch ngầm', N'Chụp X-quang, lên kế hoạch tiểu phẫu', 100000.00, GETDATE(), N'Kê đơn thuốc giảm đau');");
            migrationBuilder.Sql("SET IDENTITY_INSERT DentalRecords OFF;");

            // Seed Payments
            migrationBuilder.Sql("SET IDENTITY_INSERT Payments ON;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Payments WHERE Id = 1) INSERT INTO Payments (Id, PatientId, AppointmentId, ServiceId, Amount, PaymentDate, PaymentMethod, Status, CreatedAt) VALUES (1, 2, 24, 2, 200000.00, '2025-05-26T09:35:00', N'Tiền mặt', N'Đã thanh toán', GETDATE());");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM Payments WHERE Id = 2) INSERT INTO Payments (Id, PatientId, AppointmentId, ServiceId, Amount, PaymentDate, PaymentMethod, Status, CreatedAt) VALUES (2, 3, 25, NULL, 100000.00, '2025-05-27T11:05:00', N'Chuyển khoản', N'Đã thanh toán', GETDATE());");
            migrationBuilder.Sql("SET IDENTITY_INSERT Payments OFF;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seeded data if needed, for simplicity this is often left out for seed data
            // or would require specific DELETE statements in reverse order.

            migrationBuilder.Sql("DELETE FROM Payments WHERE Id IN (1, 2);");
            migrationBuilder.Sql("DELETE FROM DentalRecords WHERE Id IN (1, 2);");
            migrationBuilder.Sql("DELETE FROM Appointments WHERE Id IN (24, 25);");
            migrationBuilder.Sql("DELETE FROM Services WHERE Id IN (1, 2, 3);"); // Assuming service Id 1 was also part of this seed batch for consistency
            migrationBuilder.Sql("DELETE FROM UserRoles WHERE UserId = N'dentist1' AND RoleId = N'2';");
            migrationBuilder.Sql("DELETE FROM Users WHERE Id = N'dentist1';");
            migrationBuilder.Sql("DELETE FROM UserRoles WHERE UserId = N'dentist2' AND RoleId = N'2';");
            migrationBuilder.Sql("DELETE FROM Users WHERE Id = N'dentist2';");
            migrationBuilder.Sql("DELETE FROM UserRoles WHERE UserId = N'staff1' AND RoleId = N'3';");
            migrationBuilder.Sql("DELETE FROM Users WHERE Id = N'staff1';");

            // Revert admin user update (already in original Down method)
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30a25234-d565-48ba-85cd-ab1b8b6cda0b", new DateTime(2025, 5, 23, 22, 50, 45, 945, DateTimeKind.Local).AddTicks(4778), "AQAAAAIAAYagAAAAELtkySmLhPPqHNXJ4ONVt0CGkOAtPUeWkQNH4gTyP0gAi5U0l/ru8+lWhwXHbaFTgQ==", "65d75c4a-77de-4d69-916e-02481dafaec4" });
        }
    }
}

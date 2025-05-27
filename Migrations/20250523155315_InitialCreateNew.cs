using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b335936d-f690-4ee7-bd8c-d349c70809c7", new DateTime(2025, 5, 23, 22, 53, 14, 192, DateTimeKind.Local).AddTicks(6348), "AQAAAAIAAYagAAAAEPMRh2b+dn5mLWQGmXPxfH2ksLo7H0IiAM8fNFPhMp/l1NP2veVRUWhd+PHHmisQqg==", "6c57f019-1ab4-423b-b53e-09bb24aad2e3" });

            // Seed data from localhost (1).sql
            migrationBuilder.Sql("SET IDENTITY_INSERT Patients ON;");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (1, N'Nguyễn Thị Ngọc Trâm', N'ngoctram_forma@gmail.com', N'0123456689', N'HCM', '2015-05-14', N'Nữ', NULL, NULL, '2025-05-19T14:22:23.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (2, N'Trần Văn Minh', N'tranminh98@gmail.com', N'0912345678', N'Hà Nội', '1998-03-22', N'Nam', NULL, NULL, '2025-05-19T14:30:00.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (3, N'Lê Thị Bích Hạnh', N'bichhanh.le@gmail.com', N'0934567890', N'Đà Nẵng', '2001-11-10', N'Nữ', NULL, NULL, '2025-05-19T14:30:01.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (4, N'Phạm Quốc Anh', N'quocanh.pham@gmail.com', N'0901122334', N'Cần Thơ', '1995-07-01', N'Nam', NULL, NULL, '2025-05-19T14:30:02.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (5, N'Đỗ Hoàng Yến', N'yen.dh@gmail.com', N'0944455566', N'Hải Phòng', '2007-12-15', N'Nữ', NULL, NULL, '2025-05-19T14:30:03.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (6, N'Ngô Thái Bình', N'binh.ngo98@gmail.com', N'0987665544', N'HCM', '1990-06-09', N'Nam', NULL, NULL, '2025-05-19T14:30:04.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (7, N'Vũ Hương Giang', N'huonggiangvu@gmail.com', N'0922334455', N'Huế', '1988-09-19', N'Nữ', NULL, NULL, '2025-05-19T14:30:05.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (8, N'Lý Văn Tú', N'lytuvan89@gmail.com', N'0978998877', N'Bình Dương', '1989-01-05', N'Nam', NULL, NULL, '2025-05-19T14:30:06.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (9, N'Nguyễn Thị Mai Phương', N'maiphuongnt@gmail.com', N'0966123456', N'Nha Trang', '2000-04-27', N'Nữ', NULL, NULL, '2025-05-19T14:30:07.000000', NULL);");
            migrationBuilder.Sql("INSERT INTO Patients (Id, FullName, Email, PhoneNumber, Address, DateOfBirth, Gender, MedicalHistory, Allergies, CreatedAt, UpdatedAt) VALUES (10, N'Bùi Thanh Tùng', N'tungbt@gmail.com', N'0955332211', N'Vũng Tàu', '1993-02-14', N'Nam', NULL, NULL, '2025-05-19T14:30:08.000000', NULL);");
            migrationBuilder.Sql("SET IDENTITY_INSERT Patients OFF;");

            migrationBuilder.Sql("SET IDENTITY_INSERT Activities ON;");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (19, '2025-05-19T20:58:32.245543', N'Created new appointment for patient: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (20, '2025-05-19T20:59:13.520620', N'Created new appointment for patient: Nguyễn Thị Ngọc Trâm', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (21, '2025-05-19T21:03:25.073228', N'Created new appointment for patient: Hoàng Văn Cường', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (22, '2025-05-19T21:03:44.537266', N'Created new appointment for patient: Ngô Thái Bình', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (23, '2025-05-19T21:09:17.546208', N'Created new appointment for patient: Vũ Hương Giang', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (24, '2025-05-19T21:09:48.244738', N'Created new appointment for patient: Ngô Thái Bình', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (25, '2025-05-19T21:13:36.482188', N'Created new appointment for patient: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (26, '2025-05-19T21:19:37.550920', N'Created new appointment for patient: Trần Văn Minh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (27, '2025-05-19T21:35:56.798393', N'Created new appointment for patient: Ngô Thái Bình', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (28, '2025-05-19T21:37:21.241192', N'Created new appointment for patient: Trần Văn Minh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (29, '2025-05-19T21:41:22.014564', N'Created new appointment for patient: Đỗ Hoàng Yến', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (30, '2025-05-19T21:41:46.162538', N'Created new appointment for patient: Trần Mỹ Duyên', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (31, '2025-05-19T21:42:26.574880', N'Created new appointment for patient: Ngô Thái Bình', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (32, '2025-05-19T21:49:36.731347', N'Created new appointment for patient: Nguyễn Thị Mai Phương', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (33, '2025-05-19T21:53:01.750852', N'Created new appointment for patient: Vũ Hương Giang', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (34, '2025-05-19T21:57:30.329979', N'Created new appointment for patient: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (35, '2025-05-20T01:00:02.498905', N'Created new appointment for patient: Trần Văn Minh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (36, '2025-05-20T01:09:19.432222', N'Created new appointment for patient: Trần Văn Minh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (37, '2025-05-20T01:13:49.393780', N'Created new appointment for patient: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (38, '2025-05-20T01:13:57.103539', N'Created new appointment for patient: Nguyễn Thị Ngọc Trâm', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (39, '2025-05-23T08:41:48.530855', N'Created new appointment for patient: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (40, '2025-05-23T08:41:56.548606', N'Created new appointment for patient: Phạm Quốc Anh', N'1');");
            migrationBuilder.Sql("INSERT INTO Activities (Id, Time, Description, UserId) VALUES (41, '2025-05-23T08:42:00.573144', N'Đánh dấu hoàn thành lịch hẹn cho bệnh nhân: Lê Thị Bích Hạnh', N'1');");
            migrationBuilder.Sql("SET IDENTITY_INSERT Activities OFF;");

            // Seed data for Services
            migrationBuilder.Sql("SET IDENTITY_INSERT Services ON;");
            migrationBuilder.Sql("INSERT INTO Services (Id, Name, Description, Price, CreatedAt, UpdatedAt, Category, CreatedBy, Duration, IsActive) VALUES (1, N\'Trám răng\', N\'\', 150000.00, \'2025-05-19T16:26:46.0000000\', NULL, N\'Dịch vụ\', N\'admin\', \'00:20:00\', 1);");
            // Thêm các dịch vụ khác nếu có, theo mẫu trên
            // Ví dụ:
            // migrationBuilder.Sql("INSERT INTO Services (Id, Name, Description, Price, CreatedAt, UpdatedAt, Category, CreatedBy, Duration, IsActive) VALUES (2, N\'Lấy cao răng\', N\'Làm sạch mảng bám và cao răng\', 200000.00, \'2025-05-20T10:00:00.0000000\', NULL, N\'Dịch vụ\', N\'admin\', \'00:30:00\', 1);");
            migrationBuilder.Sql("SET IDENTITY_INSERT Services OFF;");

            migrationBuilder.Sql("SET IDENTITY_INSERT Appointments ON;");
            // Only seeding appointment with DentistId = '1' due to potential FK issues with other users not being seeded by default.
            // Also, ensure Service with ServiceId=1 exists.
            migrationBuilder.Sql("INSERT INTO Appointments (Id, PatientId, DentistId, AppointmentDate, StartTime, EndTime, Status, Notes, TreatmentType, CreatedAt, UpdatedAt, CreatedByUserId, ServiceId) VALUES (1, 1, N'1', '2025-05-18T17:26:29.000000', '18:26:29', '20:26:29', N'Done', NULL, N'Trám răng', '2025-05-18T17:26:29.000000', '2025-05-19T17:26:29.000000', N'1', 1);");
            migrationBuilder.Sql("SET IDENTITY_INSERT Appointments OFF;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30a25234-d565-48ba-85cd-ab1b8b6cda0b", new DateTime(2025, 5, 23, 22, 50, 45, 945, DateTimeKind.Local).AddTicks(4778), "AQAAAAIAAYagAAAAELtkySmLhPPqHNXJ4ONVt0CGkOAtPUeWkQNH4gTyP0gAi5U0l/ru8+lWhwXHbaFTgQ==", "65d75c4a-77de-4d69-916e-02481dafaec4" });
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DentalRecord> DentalRecords { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set default collation for the database to support UTF-8
            builder.UseCollation("Latin1_General_100_CI_AS_SC_UTF8");

            // Rename tables
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // Configure relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Dentist)
                .WithMany()
                .HasForeignKey(a => a.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DentalRecord>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.DentalRecords)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DentalRecord>()
                .HasOne(d => d.Dentist)
                .WithMany()
                .HasForeignKey(d => d.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fix multiple cascade paths
            builder.Entity<Payment>()
                .HasOne(p => p.Patient)
                .WithMany(pt => pt.Payments) // Specify the collection navigation property on Patient
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Service)
                .WithMany()
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Appointment)
                .WithMany()
                .HasForeignKey(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure DateOfBirth to use date type
            builder.Entity<ApplicationUser>().Property(u => u.DateOfBirth).HasColumnType("date");
            builder.Entity<Patient>().Property(p => p.DateOfBirth).HasColumnType("date");

            // Configure decimal types
            builder.Entity<DentalRecord>().Property(p => p.Cost).HasColumnType("decimal(18,2)");
            builder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Entity<Service>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<PaymentTransaction>().Property(p => p.Amount).HasColumnType("decimal(18,2)");

            // Seed admin role
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Dentist", NormalizedName = "DENTIST" },
                new IdentityRole { Id = "3", Name = "Staff", NormalizedName = "STAFF" }
            );

            // Seed admin user
            var adminUser = new ApplicationUser
            {
                Id = "1",
                UserName = "admin@dental.com",
                NormalizedUserName = "ADMIN@DENTAL.COM",
                Email = "admin@dental.com",
                NormalizedEmail = "ADMIN@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "System Administrator",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to admin user
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
            );

            // Seed Dentist Users (4 dentists)
            var dentist1 = new ApplicationUser
            {
                Id = "2",
                UserName = "dentist1@dental.com",
                NormalizedUserName = "DENTIST1@DENTAL.COM",
                Email = "dentist1@dental.com",
                NormalizedEmail = "DENTIST1@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Dr. Nguyễn Văn A",
                DateOfBirth = new DateOnly(1985, 5, 15),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            dentist1.PasswordHash = passwordHasher.HashPassword(dentist1, "Dentist@123");

            var dentist2 = new ApplicationUser
            {
                Id = "3",
                UserName = "dentist2@dental.com",
                NormalizedUserName = "DENTIST2@DENTAL.COM",
                Email = "dentist2@dental.com",
                NormalizedEmail = "DENTIST2@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Dr. Trần Thị B",
                DateOfBirth = new DateOnly(1987, 3, 20),
                Gender = "Female",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            dentist2.PasswordHash = passwordHasher.HashPassword(dentist2, "Dentist@123");

            var dentist3 = new ApplicationUser
            {
                Id = "4",
                UserName = "dentist3@dental.com",
                NormalizedUserName = "DENTIST3@DENTAL.COM",
                Email = "dentist3@dental.com",
                NormalizedEmail = "DENTIST3@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Dr. Lê Văn C",
                DateOfBirth = new DateOnly(1989, 7, 10),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            dentist3.PasswordHash = passwordHasher.HashPassword(dentist3, "Dentist@123");

            var dentist4 = new ApplicationUser
            {
                Id = "5",
                UserName = "dentist4@dental.com",
                NormalizedUserName = "DENTIST4@DENTAL.COM",
                Email = "dentist4@dental.com",
                NormalizedEmail = "DENTIST4@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Dr. Phạm Thị D",
                DateOfBirth = new DateOnly(1991, 9, 25),
                Gender = "Female",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            dentist4.PasswordHash = passwordHasher.HashPassword(dentist4, "Dentist@123");

            // Seed Staff Users (3 staff)
            var staff1 = new ApplicationUser
            {
                Id = "6",
                UserName = "staff1@dental.com",
                NormalizedUserName = "STAFF1@DENTAL.COM",
                Email = "staff1@dental.com",
                NormalizedEmail = "STAFF1@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Nguyễn Thị E",
                DateOfBirth = new DateOnly(1993, 2, 14),
                Gender = "Female",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            staff1.PasswordHash = passwordHasher.HashPassword(staff1, "Staff@123");

            var staff2 = new ApplicationUser
            {
                Id = "7",
                UserName = "staff2@dental.com",
                NormalizedUserName = "STAFF2@DENTAL.COM",
                Email = "staff2@dental.com",
                NormalizedEmail = "STAFF2@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Trần Văn F",
                DateOfBirth = new DateOnly(1994, 6, 8),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            staff2.PasswordHash = passwordHasher.HashPassword(staff2, "Staff@123");

            var staff3 = new ApplicationUser
            {
                Id = "8",
                UserName = "staff3@dental.com",
                NormalizedUserName = "STAFF3@DENTAL.COM",
                Email = "staff3@dental.com",
                NormalizedEmail = "STAFF3@DENTAL.COM",
                EmailConfirmed = true,
                FullName = "Lê Thị G",
                DateOfBirth = new DateOnly(1995, 11, 30),
                Gender = "Female",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };
            staff3.PasswordHash = passwordHasher.HashPassword(staff3, "Staff@123");

            builder.Entity<ApplicationUser>().HasData(dentist1, dentist2, dentist3, dentist4, staff1, staff2, staff3);

            // Assign dentist roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "2", UserId = "2" },
                new IdentityUserRole<string> { RoleId = "2", UserId = "3" },
                new IdentityUserRole<string> { RoleId = "2", UserId = "4" },
                new IdentityUserRole<string> { RoleId = "2", UserId = "5" },
                new IdentityUserRole<string> { RoleId = "3", UserId = "6" },
                new IdentityUserRole<string> { RoleId = "3", UserId = "7" },
                new IdentityUserRole<string> { RoleId = "3", UserId = "8" }
            );

            // Seed Services (10 services)
            builder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Khám nha", Category = "Khám", Description = "Kiểm tra toàn bộ răng và nướu", Price = 150000, DurationMinutes = 30, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 2, Name = "Vệ sinh răng", Category = "Làm sạch", Description = "Lấy cao răng và vệ sinh toàn bộ", Price = 300000, DurationMinutes = 45, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 3, Name = "Trám răng", Category = "Điều trị", Description = "Trám sâu răng với vật liệu composite", Price = 200000, DurationMinutes = 45, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 4, Name = "Nhổ răng", Category = "Điều trị", Description = "Nhổ răng hư hỏng nặng", Price = 250000, DurationMinutes = 60, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 5, Name = "Niềng răng", Category = "Niềng răng", Description = "Niềng răng để chỉnh sửa hàm", Price = 5000000, DurationMinutes = 120, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 6, Name = "Tẩy trắng răng", Category = "Làm sạch", Description = "Tẩy trắng an toàn cho răng", Price = 400000, DurationMinutes = 60, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 7, Name = "Nhân tạo implant", Category = "Trồng implant", Description = "Cấy ghép implant để thay thế răng mất", Price = 10000000, DurationMinutes = 180, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 8, Name = "Tiêm botox nướu", Category = "Làm sạch", Description = "Tiêm botox chuyên biệt cho nướu", Price = 350000, DurationMinutes = 30, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 9, Name = "Xử lý viêm nướu", Category = "Điều trị", Description = "Điều trị viêm nướu và chảy máu", Price = 250000, DurationMinutes = 45, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" },
                new Service { Id = 10, Name = "Bọc sứ răng", Category = "Điều trị", Description = "Bọc sứ để phục hình răng hỏng", Price = 800000, DurationMinutes = 90, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "1" }
            );

            // Seed Patients (10 patients)
            builder.Entity<Patient>().HasData(
                new Patient { Id = 1, FullName = "Nguyễn Văn An", Email = "an.nguyen@example.com", PhoneNumber = "0901234567", DateOfBirth = new DateOnly(1990, 1, 15), Gender = "Nam", Address = "123 Đường ABC, Quận 1, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 2, FullName = "Trần Thị Bích", Email = "bich.tran@example.com", PhoneNumber = "0902345678", DateOfBirth = new DateOnly(1992, 5, 20), Gender = "Nữ", Address = "456 Đường XYZ, Quận 2, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 3, FullName = "Lê Văn Công", Email = "cong.le@example.com", PhoneNumber = "0903456789", DateOfBirth = new DateOnly(1988, 3, 10), Gender = "Nam", Address = "789 Đường MNO, Quận 3, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 4, FullName = "Phạm Thị Dung", Email = "dung.pham@example.com", PhoneNumber = "0904567890", DateOfBirth = new DateOnly(1995, 7, 25), Gender = "Nữ", Address = "321 Đường PQR, Quận 4, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 5, FullName = "Hoàng Văn Em", Email = "em.hoang@example.com", PhoneNumber = "0905678901", DateOfBirth = new DateOnly(1991, 9, 8), Gender = "Nam", Address = "654 Đường STU, Quận 5, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 6, FullName = "Vũ Thị Hương", Email = "huong.vu@example.com", PhoneNumber = "0906789012", DateOfBirth = new DateOnly(1993, 11, 12), Gender = "Nữ", Address = "987 Đường VWX, Quận 6, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 7, FullName = "Đặng Văn Ích", Email = "ich.dang@example.com", PhoneNumber = "0907890123", DateOfBirth = new DateOnly(1989, 2, 18), Gender = "Nam", Address = "147 Đường YZA, Quận 7, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 8, FullName = "Ngô Thị Khánh", Email = "khanh.ngo@example.com", PhoneNumber = "0908901234", DateOfBirth = new DateOnly(1996, 4, 30), Gender = "Nữ", Address = "258 Đường BCD, Quận 8, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 9, FullName = "Tạ Văn Lâm", Email = "lam.ta@example.com", PhoneNumber = "0909012345", DateOfBirth = new DateOnly(1987, 6, 5), Gender = "Nam", Address = "369 Đường EFG, Quận 9, TP.HCM", IsActive = true, CreatedAt = DateTime.Now },
                new Patient { Id = 10, FullName = "Dương Thị Mộng", Email = "mong.duong@example.com", PhoneNumber = "0900123456", DateOfBirth = new DateOnly(1994, 8, 22), Gender = "Nữ", Address = "741 Đường HIJ, Quận 10, TP.HCM", IsActive = true, CreatedAt = DateTime.Now }
            );

            // Seed Appointments (10 appointments)
            var now = DateTime.Now;
            builder.Entity<Appointment>().HasData(
                new Appointment { Id = 1, PatientId = 1, DentistId = "2", ServiceId = 1, AppointmentDate = now.AddDays(1), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(9, 30, 0), Status = "Scheduled", PatientName = "Nguyễn Văn An", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 2, PatientId = 2, DentistId = "3", ServiceId = 2, AppointmentDate = now.AddDays(1), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(10, 45, 0), Status = "Scheduled", PatientName = "Trần Thị Bích", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 3, PatientId = 3, DentistId = "4", ServiceId = 3, AppointmentDate = now.AddDays(2), StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(11, 45, 0), Status = "Scheduled", PatientName = "Lê Văn Công", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 4, PatientId = 4, DentistId = "5", ServiceId = 4, AppointmentDate = now.AddDays(2), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 0, 0), Status = "Scheduled", PatientName = "Phạm Thị Dung", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 5, PatientId = 5, DentistId = "2", ServiceId = 5, AppointmentDate = now.AddDays(3), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), Status = "Scheduled", PatientName = "Hoàng Văn Em", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 6, PatientId = 6, DentistId = "3", ServiceId = 6, AppointmentDate = now.AddDays(3), StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "Completed", PatientName = "Vũ Thị Hương", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 7, PatientId = 7, DentistId = "4", ServiceId = 7, AppointmentDate = now.AddDays(4), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = "Scheduled", PatientName = "Đặng Văn Ích", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 8, PatientId = 8, DentistId = "5", ServiceId = 8, AppointmentDate = now.AddDays(5), StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(13, 30, 0), Status = "Scheduled", PatientName = "Ngô Thị Khánh", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 9, PatientId = 9, DentistId = "2", ServiceId = 9, AppointmentDate = now.AddDays(5), StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(16, 45, 0), Status = "Scheduled", PatientName = "Tạ Văn Lâm", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" },
                new Appointment { Id = 10, PatientId = 10, DentistId = "3", ServiceId = 10, AppointmentDate = now.AddDays(6), StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(12, 30, 0), Status = "Scheduled", PatientName = "Dương Thị Mộng", CreatedAt = DateTime.Now, CreatedByUserId = "1", CreatedBy = "System" }
            );

        }
    }
}
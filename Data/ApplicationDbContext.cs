using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;

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
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
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

            // Seed admin role
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Dentist", NormalizedName = "DENTIST" },
                new IdentityRole { Id = "3", Name = "Staff", NormalizedName = "STAFF" },
                new IdentityRole { Id = "4", Name = "User", NormalizedName = "USER" }
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
        }
    }
}
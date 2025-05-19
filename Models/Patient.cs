using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Allergies { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DentalRecord> DentalRecords { get; set; } = new List<DentalRecord>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Alias for FullName to maintain compatibility
        public string Name => FullName;
    }
} 
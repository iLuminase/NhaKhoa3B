using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string DentistId { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled, No-show

        public string? Notes { get; set; }
        public string? TreatmentType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public ApplicationUser Dentist { get; set; } = null!;
        public ApplicationUser CreatedByUser { get; set; } = null!;
    }
} 
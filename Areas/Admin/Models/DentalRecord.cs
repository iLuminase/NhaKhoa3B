using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Areas.Admin.Models
{
    public class DentalRecord
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string DentistId { get; set; } = string.Empty;

        [Required]
        public DateTime TreatmentDate { get; set; }

        [Required]
        public string TreatmentType { get; set; } = string.Empty;

        public string? Diagnosis { get; set; }
        public string? TreatmentNotes { get; set; }
        public string? Prescription { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public ApplicationUser Dentist { get; set; } = null!;
    }
} 
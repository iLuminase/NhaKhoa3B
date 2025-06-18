using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Areas.Admin.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
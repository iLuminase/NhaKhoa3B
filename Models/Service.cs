using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá dịch vụ là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá dịch vụ phải lớn hơn 0")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; }

        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Foreign key for ServiceCategory
        public int? ServiceCategoryId { get; set; }

        // Navigation property
        public virtual ServiceCategory? ServiceCategory { get; set; }
    }
}
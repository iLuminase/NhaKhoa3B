using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Areas.Admin.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Duration in minutes
        /// </summary>
        [Range(1, 480, ErrorMessage = "Thời gian phải từ 1 đến 480 phút")]
        public int DurationMinutes { get; set; } = 30;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Helper property for display
        public string DurationDisplay => $"{DurationMinutes / 60:D2}h {DurationMinutes % 60:D2}m";
    }
}
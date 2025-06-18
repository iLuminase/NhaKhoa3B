using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Areas.Admin.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
    }
} 
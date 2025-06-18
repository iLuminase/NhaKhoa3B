using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.ViewModels
{
    public class PublicRegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
    }
}

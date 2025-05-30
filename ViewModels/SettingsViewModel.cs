using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.ViewModels
{
    public class SettingsViewModel
    {
        [Display(Name = "Nhận thông báo email")]
        public bool EmailNotifications { get; set; } = true;

        [Display(Name = "Nhận thông báo SMS")]
        public bool SmsNotifications { get; set; } = false;

        [Display(Name = "Hiển thị thông tin cá nhân")]
        public bool ShowPersonalInfo { get; set; } = true;

        [Display(Name = "Ngôn ngữ")]
        public string Language { get; set; } = "vi";

        [Display(Name = "Múi giờ")]
        public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";

        [Display(Name = "Chế độ tối")]
        public bool DarkMode { get; set; } = false;

        [Display(Name = "Số lượng bản ghi hiển thị mỗi trang")]
        [Range(10, 100, ErrorMessage = "Số lượng phải từ 10 đến 100")]
        public int PageSize { get; set; } = 20;

        [Display(Name = "Tự động đăng xuất sau (phút)")]
        [Range(5, 480, ErrorMessage = "Thời gian phải từ 5 đến 480 phút")]
        public int AutoLogoutMinutes { get; set; } = 60;

        [Display(Name = "Xác thực hai yếu tố")]
        public bool TwoFactorEnabled { get; set; } = false;
    }
}

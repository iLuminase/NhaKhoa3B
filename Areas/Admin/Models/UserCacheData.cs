namespace MyMvcApp.Areas.Admin.Models
{
    public class UserCacheData
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> RoleNames { get; set; } = new();
        public bool IsActive { get; set; }
    }
} 
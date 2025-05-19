using System;
using System.Collections.Generic;

namespace MyMvcApp.ViewModels
{
    public class UserManagementViewModel
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public required List<string> RoleNames { get; set; }
    }
} 
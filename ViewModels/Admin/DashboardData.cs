namespace MyMvcApp.ViewModels.Admin
{
    public class DashboardData
    {
        public int TotalAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int TotalServices { get; set; }
        public decimal TotalRevenue { get; set; }
        public IEnumerable<object> RecentAppointments { get; set; } = new List<object>();
        public IEnumerable<object> UpcomingAppointments { get; set; } = new List<object>();

        // Thêm các property còn thiếu cho dashboard
        public int TodayAppointments { get; set; }
        public int TotalUsers { get; set; }
        public IEnumerable<object> RecentActivities { get; set; } = new List<object>();
    }
} 
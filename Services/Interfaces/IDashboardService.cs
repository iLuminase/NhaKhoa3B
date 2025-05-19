using MyMvcApp.ViewModels.Admin;

namespace MyMvcApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetTotalAppointmentsAsync();
        Task<int> GetTotalPatientsAsync();
        Task<int> GetTotalServicesAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<IEnumerable<object>> GetRecentAppointmentsAsync();
        Task<IEnumerable<object>> GetUpcomingAppointmentsAsync();
        Task<DashboardData> GetDashboardDataAsync();
    }
} 
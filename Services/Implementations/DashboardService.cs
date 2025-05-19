using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.ViewModels.Admin;

namespace MyMvcApp.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalAppointmentsAsync()
        {
            return await _context.Appointments.CountAsync();
        }

        public async Task<int> GetTotalPatientsAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> GetTotalServicesAsync()
        {
            return await _context.Services.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Payments.SumAsync(p => p.Amount);
        }

        public async Task<IEnumerable<object>> GetRecentAppointmentsAsync()
        {
            return await _context.Appointments
                .OrderByDescending(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Status,
                    PatientName = a.Patient.FullName,
                    ServiceName = a.Service.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetUpcomingAppointmentsAsync()
        {
            var today = DateTime.Today;
            return await _context.Appointments
                .Where(a => a.AppointmentDate >= today)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Status,
                    PatientName = a.Patient.FullName,
                    ServiceName = a.Service.Name
                })
                .ToListAsync();
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var totalAppointments = await GetTotalAppointmentsAsync();
            var totalPatients = await GetTotalPatientsAsync();
            var totalServices = await GetTotalServicesAsync();
            var totalRevenue = await GetTotalRevenueAsync();
            var recentAppointments = await GetRecentAppointmentsAsync();
            var upcomingAppointments = await GetUpcomingAppointmentsAsync();

            return new DashboardData
            {
                TotalAppointments = totalAppointments,
                TotalPatients = totalPatients,
                TotalServices = totalServices,
                TotalRevenue = totalRevenue,
                RecentAppointments = recentAppointments,
                UpcomingAppointments = upcomingAppointments
            };
        }
    }
} 
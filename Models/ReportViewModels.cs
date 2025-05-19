using System;

namespace MyMvcApp.Models
{
    public class RevenueReportViewModel
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ServiceReportViewModel
    {
        public required string ServiceName { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class PatientReportViewModel
    {
        public required string PatientName { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalPayments { get; set; }
        public DateTime? LastVisit { get; set; }
    }

    public class StaffReportViewModel
    {
        public required string StaffName { get; set; }
        public int TotalActivities { get; set; }
        public DateTime LastActivity { get; set; }
    }

    public class ReportViewModel
    {
        public required string ServiceName { get; set; }
        public required string PatientName { get; set; }
        public required string StaffName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
} 
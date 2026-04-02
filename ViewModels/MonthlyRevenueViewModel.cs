using System;
using System.Collections.Generic;

namespace MyMvcApp.ViewModels
{
    public class MonthlyRevenueViewModel
    {
        public string Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalAppointments { get; set; }
        public List<DailyRevenue> DailyBreakdown { get; set; }
    }

    public class DailyRevenue
    {
        public int Day { get; set; }
        public decimal Revenue { get; set; }
    }
}

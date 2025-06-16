using MyMvcApp.Models;

namespace MyMvcApp.ViewModels.Admin
{
    public class PaymentAppointmentViewModel
    {
        public Appointment Appointment { get; set; } = new Appointment();
        public string PaymentStatusString { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
    }
}

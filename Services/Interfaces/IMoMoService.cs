using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Services.Interfaces
{
    public interface IMoMoService
    {
        Task<MoMoPaymentResponse> CreatePaymentAsync(int appointmentId, decimal amount, string orderInfo);
        Task<bool> VerifyPaymentAsync(string orderId, string requestId, string signature);
        string GenerateQRCode(string payUrl);
        Task<PaymentTransaction> CreatePaymentTransactionAsync(int appointmentId, decimal amount, string paymentMethod);
        Task<PaymentTransaction?> GetPaymentTransactionAsync(string orderId);
        Task<bool> UpdatePaymentStatusAsync(string orderId, string status, string? transactionId = null);
    }
}

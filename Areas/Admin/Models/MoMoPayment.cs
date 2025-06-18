using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Areas.Admin.Models
{
    public class MoMoPaymentRequest
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string OrderInfo { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public string IpnUrl { get; set; } = string.Empty;
        public string ExtraData { get; set; } = string.Empty;
        public string RequestType { get; set; } = "captureWallet";
        public string Signature { get; set; } = string.Empty;
        public string Lang { get; set; } = "vi";
    }

    public class MoMoPaymentResponse
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public long ResponseTime { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ResultCode { get; set; } = string.Empty;
        public string PayUrl { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
        public string Deeplink { get; set; } = string.Empty;
        public string DeeplinkMiniApp { get; set; } = string.Empty;
    }

    public class MoMoConfig
    {
        public string PartnerCode { get; set; } = "MOMO";
        public string AccessKey { get; set; } = "F8BBA842ECF85";
        public string SecretKey { get; set; } = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
        public string Endpoint { get; set; } = "https://test-payment.momo.vn/v2/gateway/api/create";
        public string ReturnUrl { get; set; } = string.Empty;
        public string NotifyUrl { get; set; } = string.Empty;
    }

    public class PaymentTransaction
    {
        public int Id { get; set; }
        
        [Required]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        
        [Required]
        public string OrderId { get; set; } = string.Empty;
        
        [Required]
        public string RequestId { get; set; } = string.Empty;
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; } = string.Empty; // MoMo, Cash, Card
        
        public string? TransactionId { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Cancelled
        public string? MoMoTransactionId { get; set; }
        public string? PayUrl { get; set; }
        public string? QrCodeUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}

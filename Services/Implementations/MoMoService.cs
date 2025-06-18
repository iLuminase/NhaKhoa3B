using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MyMvcApp.Data;
using MyMvcApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Services.Implementations
{
    public class MoMoService : IMoMoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly MoMoConfig _momoConfig;

        public MoMoService(ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
            _momoConfig = new MoMoConfig
            {
                PartnerCode = "MOMO",
                AccessKey = "F8BBA842ECF85",
                SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz",
                Endpoint = "https://test-payment.momo.vn/v2/gateway/api/create",
                ReturnUrl = _configuration["MoMo:ReturnUrl"] ?? "https://localhost:7000/Payment/PaymentReturn",
                NotifyUrl = _configuration["MoMo:NotifyUrl"] ?? "https://localhost:7000/Payment/PaymentNotify"
            };
        }

        public async Task<MoMoPaymentResponse> CreatePaymentAsync(int appointmentId, decimal amount, string orderInfo)
        {
            var orderId = $"ORDER_{appointmentId}_{DateTime.Now:yyyyMMddHHmmss}";
            var requestId = Guid.NewGuid().ToString();

            var request = new MoMoPaymentRequest
            {
                PartnerCode = _momoConfig.PartnerCode,
                RequestId = requestId,
                OrderId = orderId,
                Amount = (long)amount,
                OrderInfo = orderInfo,
                RedirectUrl = _momoConfig.ReturnUrl,
                IpnUrl = _momoConfig.NotifyUrl,
                ExtraData = "",
                RequestType = "captureWallet",
                Lang = "vi"
            };

            // Tạo signature
            var rawSignature = $"accessKey={_momoConfig.AccessKey}&amount={request.Amount}&extraData={request.ExtraData}&ipnUrl={request.IpnUrl}&orderId={request.OrderId}&orderInfo={request.OrderInfo}&partnerCode={request.PartnerCode}&redirectUrl={request.RedirectUrl}&requestId={request.RequestId}&requestType={request.RequestType}";
            request.Signature = ComputeHmacSha256(rawSignature, _momoConfig.SecretKey);

            try
            {
                // Tạo demo response thay vì gọi API thật (để test)
                var demoPayUrl = $"https://test-payment.momo.vn/gw_payment/transactionProcessor?partnerCode={request.PartnerCode}&orderId={request.OrderId}&amount={request.Amount}";

                var momoResponse = new MoMoPaymentResponse
                {
                    PartnerCode = request.PartnerCode,
                    RequestId = request.RequestId,
                    OrderId = request.OrderId,
                    Amount = request.Amount,
                    ResponseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Message = "Success",
                    ResultCode = "0",
                    PayUrl = demoPayUrl,
                    QrCodeUrl = "", // Sẽ được tạo bên dưới
                    Deeplink = $"momo://payment?orderId={request.OrderId}",
                    DeeplinkMiniApp = ""
                };

                // Uncomment dòng dưới để gọi API MoMo thật
                /*
                var jsonRequest = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_momoConfig.Endpoint, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                momoResponse = JsonSerializer.Deserialize<MoMoPaymentResponse>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                */

                if (momoResponse != null && momoResponse.ResultCode == "0")
                {
                    // Đảm bảo luôn có QR code URL
                    if (string.IsNullOrEmpty(momoResponse.QrCodeUrl) && !string.IsNullOrEmpty(momoResponse.PayUrl))
                    {
                        // Tạo QR code bằng QR Server API
                        momoResponse.QrCodeUrl = GenerateQRServerUrl(momoResponse.PayUrl);
                    }

                    // Lưu transaction vào database
                    var transaction = new PaymentTransaction
                    {
                        AppointmentId = appointmentId,
                        OrderId = orderId,
                        RequestId = requestId,
                        Amount = amount,
                        PaymentMethod = "MoMo",
                        Status = "Pending",
                        PayUrl = momoResponse.PayUrl,
                        QrCodeUrl = momoResponse.QrCodeUrl,
                        CreatedAt = DateTime.Now
                    };

                    _context.PaymentTransactions.Add(transaction);
                    await _context.SaveChangesAsync();
                }

                return momoResponse ?? new MoMoPaymentResponse { ResultCode = "99", Message = "Lỗi khi tạo thanh toán" };
            }
            catch (Exception ex)
            {
                return new MoMoPaymentResponse
                {
                    ResultCode = "99",
                    Message = $"Lỗi kết nối: {ex.Message}"
                };
            }
        }

        public async Task<bool> VerifyPaymentAsync(string orderId, string requestId, string signature)
        {
            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.OrderId == orderId && t.RequestId == requestId);

            if (transaction == null) return false;

            // Verify signature logic here
            return true;
        }

        public string GenerateQRCode(string payUrl)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(payUrl, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(20);
                return Convert.ToBase64String(qrCodeBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GenerateQRCodeBase64(string payUrl)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(payUrl, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(20);
                var base64String = Convert.ToBase64String(qrCodeBytes);
                return $"data:image/png;base64,{base64String}";
            }
            catch
            {
                return GenerateQRServerUrl(payUrl);
            }
        }

        private string GenerateQRServerUrl(string payUrl)
        {
            try
            {
                var encodedUrl = Uri.EscapeDataString(payUrl);
                return $"https://api.qrserver.com/v1/create-qr-code/?size=300x300&data={encodedUrl}";
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<PaymentTransaction> CreatePaymentTransactionAsync(int appointmentId, decimal amount, string paymentMethod)
        {
            var orderId = $"ORDER_{appointmentId}_{DateTime.Now:yyyyMMddHHmmss}";
            var requestId = Guid.NewGuid().ToString();

            var transaction = new PaymentTransaction
            {
                AppointmentId = appointmentId,
                OrderId = orderId,
                RequestId = requestId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.PaymentTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<PaymentTransaction?> GetPaymentTransactionAsync(string orderId)
        {
            return await _context.PaymentTransactions
                .Include(t => t.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(t => t.Appointment)
                .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        public async Task<bool> UpdatePaymentStatusAsync(string orderId, string status, string? transactionId = null)
        {
            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.OrderId == orderId);

            if (transaction == null) return false;

            transaction.Status = status;
            transaction.MoMoTransactionId = transactionId;

            if (status == "Success")
            {
                transaction.CompletedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

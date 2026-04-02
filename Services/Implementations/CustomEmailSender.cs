using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Services.Implementations
{
    public class CustomEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public CustomEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var portString = smtpSettings["Port"];
            var port = !string.IsNullOrEmpty(portString) ? int.Parse(portString) : 25;
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"] ?? "no-reply@example.com";

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(username, password);

                var mailMessage = new MailMessage(fromEmail, toEmail, subject, message)
                {
                    IsBodyHtml = true
                };
                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var subject = "Xác nhận tài khoản";
            var message = $"Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{confirmationLink}'>click vào đây</a>.";
            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            var subject = "Đặt lại mật khẩu của bạn";
            var message = $@"
                <p>Xin chào {user.FullName},</p>
                <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Vui lòng nhấp vào liên kết bên dưới để đặt lại mật khẩu của bạn. Liên kết này sẽ hết hạn sau 24 giờ.</p>
                <p><a href='{resetLink}' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #fff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,<br>Đội ngũ Nha Khoa 3B</p>
            ";
            await SendEmailAsync(email, subject, message);
        }
    }
}
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
            var subject = "Đặt lại mật khẩu";
            var message = $"Để đặt lại mật khẩu, vui lòng <a href='{resetLink}'>click vào đây</a>.";
            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            var subject = "Mã đặt lại mật khẩu";
            var message = $"Mã đặt lại mật khẩu của bạn là: {resetCode}";
            await SendEmailAsync(email, subject, message);
        }
    }
} 
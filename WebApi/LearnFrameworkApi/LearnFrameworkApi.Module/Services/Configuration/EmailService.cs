using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Services.Configuration
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly AppDbContext _context;

        public EmailService(SmtpClient smtpClient, AppDbContext context)
        {
            _smtpClient = smtpClient;
            _context = context;
        }

        public void SendResetPasswordLink(string to, string fullName, string randomString)
        {
            var smtpSetting = SmtpSetting.GetInstance(_context);
            var systemConfigration = SystemConfiguration.GetInstance(_context);

            string from = new MailAddress(smtpSetting.SmtpUser, "no-reply").ToString();
            string subject = "NCS App - Password Reset Request";
            string linkResetPassword = systemConfigration.AppBaseUrl + "/ResetPassword?resetToken=" + Uri.EscapeDataString(randomString);

            string body = string.Format(
                @"Dear, {0}<br/>
                We received a request to reset your password for your account associated with this email address. If you did not make this request, you can safely ignore this email. <br/>
                To reset your password, please click the link below: <br/>
                <br/>
                <a href='{1}' target='_blank'>Reset Password</a> <br/>
                <br/>
                This link will expire in 5 minutes for your security. If you encounter any issues or have any questions, feel free to contact our support team. <br/>
                Thank you for your attention. <br/>
                Best regards, <br/>
                <br/>
                <br/>
                The NCS App Team", fullName, linkResetPassword);

            var mailMessage = new MailMessage(from, to, subject, body);
            mailMessage.IsBodyHtml = true;
            _smtpClient.Send(mailMessage);
        }
    }
}

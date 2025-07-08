using HandiMaker.Data.Helper;
using HandiMaker.Services.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace HandiMaker.Services.Services.implement
{
    public class Emailresult
    {
        public bool IsSucceeded { get; set; }
        public string MessageResult { get; set; } = "";
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSetting;

        public EmailService(EmailSettings emailSetting)
        {
            _emailSetting = emailSetting;
        }
        public async Task<Emailresult> SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_emailSetting.SmtpServer, _emailSetting.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password);
                    client.EnableSsl = true; // Use SSL for security
                    client.UseDefaultCredentials = false;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSetting.FromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);

                }
                return new Emailresult { IsSucceeded = true };
            }
            catch (Exception ex)
            {
                return new Emailresult { IsSucceeded = false, MessageResult = ex.Message };
            }
        }
    }
}

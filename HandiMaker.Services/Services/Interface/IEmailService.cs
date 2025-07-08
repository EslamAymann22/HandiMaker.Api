using HandiMaker.Services.Services.implement;

namespace HandiMaker.Services.Services.Interface
{
    public interface IEmailService
    {
        public Task<Emailresult> SendEmail(string toEmail, string subject, string body);
    }
}

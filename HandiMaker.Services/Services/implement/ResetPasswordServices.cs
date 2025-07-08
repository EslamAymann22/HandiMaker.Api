
using HandiMaker.Services.Services.Interface;

namespace HandiMaker.Services.Services.implement
{
    public class ResetPasswordServices : IResetPasswordServices
    {
        private readonly IEmailService _emailService;
        private readonly ICacheServices _cacheServices;

        public ResetPasswordServices(IEmailService emailService, ICacheServices cacheServices)
        {
            this._emailService = emailService;
            this._cacheServices = cacheServices;
        }

        public async Task<bool> CheckOTPCodeAsync(string Email, string Code)
        {
            var validDTO = await _cacheServices.GetCacheAsync<string>($"OTP_{Email}");
            if (validDTO == null || validDTO != Code)
                return false;
            var result = await _cacheServices.RemoveCacheAsync($"OTP_{Email}");
            return result;
        }

        public async Task<bool> SendOTPAsync(string Email, string UserName = "User")
        {
            var CodeOTP = new Random().Next(1000, 9999).ToString();
            await _cacheServices.SetCacheAsync($"OTP_{Email}", CodeOTP);

            var body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
        <div style='max-width: 600px; margin: auto; background-color: white; border-radius: 8px; padding: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
            <h2 style='color: #333;'>🔐 Reset Your Password</h2>
            <p>Dear {UserName},</p>
            <p>We received a request to reset your password. Please use the following code to proceed:</p>
            <p style='font-size: 24px; font-weight: bold; color: #007bff; text-align: center; letter-spacing: 4px;'>{CodeOTP}</p>
            <p>This code is valid for a 5 Minutes !. If you didn’t request this, you can ignore this email.</p>
            <p>Best regards,<br><strong>HandiMaker Team</strong></p>
        </div>
    </div>";

            var res = await _emailService.SendEmail(Email, "Reset Password", body);
            return res.IsSucceeded;
        }
    }
}

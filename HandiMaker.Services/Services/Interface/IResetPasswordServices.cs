namespace HandiMaker.Services.Services.Interface
{
    public interface IResetPasswordServices
    {

        public Task<bool> SendOTPAsync(string Email, string UserName = "User");
        public Task<bool> CheckOTPCodeAsync(string Email, string Code);

    }
}

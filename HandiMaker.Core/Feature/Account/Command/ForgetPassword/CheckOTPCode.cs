using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Command.ForgetPassword
{
    public class CheckOTPCodeDto
    {
        public string? ResetToken { get; set; }
    }


    public record CheckOTPCodeModel : IRequest<BaseResponse<CheckOTPCodeDto>>
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Code { get; set; }
    }
    public class CheckOTPCodeHandler : BaseResponseHandler,
                                      IRequestHandler<CheckOTPCodeModel, BaseResponse<CheckOTPCodeDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICacheServices _cacheServices;
        private readonly IResetPasswordServices _resetPassword;

        public CheckOTPCodeHandler(UserManager<AppUser> userManager, ICacheServices cacheServices, IResetPasswordServices resetPassword)
        {
            _userManager = userManager;
            _cacheServices = cacheServices;
            this._resetPassword = resetPassword;
        }
        public async Task<BaseResponse<CheckOTPCodeDto>> Handle(CheckOTPCodeModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Failed<CheckOTPCodeDto>(System.Net.HttpStatusCode.BadRequest, "User is Not Found");

            var CheckOtp = await _resetPassword.CheckOTPCodeAsync(request.Email, request.Code);
            if (!CheckOtp)
                return Failed<CheckOTPCodeDto>(HttpStatusCode.BadRequest, "OTP Code not correct");
            var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Success(new CheckOTPCodeDto { ResetToken = ResetToken });
        }
    }
}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HandiMaker.Core.Feature.Account.Command.ForgetPassword
{
    public class ResetPasswordModel : IRequest<BaseResponse<string>>
    {
        [EmailAddress]
        public string Email { get; set; }
        public string? ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }


    public class ResetPasswordHandler : BaseResponseHandler, IRequestHandler<ResetPasswordModel, BaseResponse<string>>


    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordHandler(IAuthenticationServices authenticationServices, UserManager<AppUser> userManager)
        {
            _authenticationServices = authenticationServices;
            _userManager = userManager;
        }

        public async Task<BaseResponse<string>> Handle(ResetPasswordModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "User is Not Found");
            var ChackIsSame = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (ChackIsSame)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "New password cannot be the same as the current password.");

            var res = await _userManager.ResetPasswordAsync(user, request.ResetToken ?? "", request.NewPassword);
            if (res.Succeeded)
                return Success("Password reset successfully");


            var ErrorList = res.Errors.ToList();
            var ErrorMessage = "";
            int Errorcounter = 1;
            foreach (var error in ErrorList)
                ErrorMessage += $"{Errorcounter++} : {error.Description} ,";
            return Failed<string>(System.Net.HttpStatusCode.BadRequest, ErrorMessage);
        }
    }
}

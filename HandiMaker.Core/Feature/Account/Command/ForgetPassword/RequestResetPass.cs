using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Command.ForgetPassword
{




    public class RequestResetPassModel : IRequest<BaseResponse<string>>
    {

        [EmailAddress]
        public string Email { get; set; }

    }

    public class RequestResetPassHandler : BaseResponseHandler, IRequestHandler<RequestResetPassModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IResetPasswordServices _resetPasswordServices;

        public RequestResetPassHandler(UserManager<AppUser> userManager, IResetPasswordServices resetPasswordServices)
        {
            this._userManager = userManager;
            this._resetPasswordServices = resetPasswordServices;
        }

        public async Task<BaseResponse<string>> Handle(RequestResetPassModel request, CancellationToken cancellationToken)
        {
            var User = await _userManager.FindByEmailAsync(request.Email);
            if (User is null)
                return Failed<string>(HttpStatusCode.NotFound, "This Email is not found");

            var res = await _resetPasswordServices.SendOTPAsync(request.Email, User.UserName);

            if (res)
                return Success("OTP Code send");
            return Failed<string>(HttpStatusCode.BadRequest, "Something is wrong");

        }
    }

}

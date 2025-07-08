using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text.Json.Serialization;

namespace HandiMaker.Core.Feature.Account.Command
{
    public class ChangePasswordModel : IRequest<BaseResponse<string>>
    {
        public string userId { get; set; }
        [JsonIgnore]
        public string? AuthEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordHandler : BaseResponseHandler, IRequestHandler<ChangePasswordModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public ChangePasswordHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<BaseResponse<string>> Handle(ChangePasswordModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);

            if (user is null)
                return Failed<string>(HttpStatusCode.NotFound, "User id is not found");

            if (user.Email != request.AuthEmail)
                return Failed<string>(HttpStatusCode.Unauthorized, "You aren't account owner");

            if (request.ConfirmPassword != request.NewPassword)
                return Failed<string>(HttpStatusCode.BadRequest, "New password not match with ConfirmPassword");

            var res = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (res.Succeeded)
                return Success("Password Changed!!");

            var Errors = string.Join("; ", res.Errors.Select(E => E.Description));
            return Failed<string>(HttpStatusCode.BadRequest, Errors);

        }
    }



}

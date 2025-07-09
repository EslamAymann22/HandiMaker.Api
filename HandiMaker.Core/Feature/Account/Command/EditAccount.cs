using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Command
{
    public class EditAccountModel : RegisterSecondStepModel, IRequest<BaseResponse<string>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }

    }
    public class EditAccountHandler : BaseResponseHandler, IRequestHandler<EditAccountModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditAccountHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(EditAccountModel request, CancellationToken cancellationToken)
        {
            var User = await _userManager.FindByIdAsync(request.UserId);
            if (User is null) return Failed<string>(HttpStatusCode.NotFound, "User id isn't found");

            if (User.Email != request.AuthorizeEmail)
                return Failed<string>(HttpStatusCode.Unauthorized, "you are not Account owner");

            string? IsUsed = null;

            if (request.Email != request.AuthorizeEmail)
                IsUsed = (await _userManager.FindByEmailAsync(request.Email ?? User.Email)) is not null ? "Email" : null;
            if (request.UserName != User.UserName)
                IsUsed = (await _userManager.FindByNameAsync(request.UserName ?? User.UserName)) is not null ? "UserName" : null;

            if (IsUsed is not null)
                return Failed<string>(HttpStatusCode.Conflict, $"This {IsUsed} is used");

            User.FirstName = request.FirstName;
            User.LastName = request.LastName;
            User.PhoneNumber = request.PhoneNumber;
            User.BOD = request.BOD;
            User.Location = request.Location;
            User.Gender = request.Gender;
            User.Email = request.Email;
            User.UserName = request.UserName;

            if (request.ProfilePicture is not null)
            {
                var PicUrl = DocumentServices.UploadFile(request.ProfilePicture, FoldersName.ProfileImages.ToString(), _httpContextAccessor);
                User.PictureUrl = PicUrl;
            }

            var result = await _userManager.UpdateAsync(User);

            if (result.Succeeded)
                return Success("User Updated!!");

            var UpdateErrors = string.Join("; ", result.Errors);
            return Failed<string>(HttpStatusCode.InternalServerError, UpdateErrors);


        }
    }
}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace HandiMaker.Core.Feature.Account.Command
{

    public class RegisterSecondStepModel : IRequest<BaseResponse<string>>
    {
        [JsonIgnore]
        public string? AuthorizeEmail { get; set; }
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Phone Number is Must contain 11 digit and be Valid number")]
        [MaxLength(11, ErrorMessage = "Phone number must be exactly 11 digits")]
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public DateOnly BOD { get; set; }
        public UserGender Gender { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }

    public class RegisterSecondStepHandler : BaseResponseHandler, IRequestHandler<RegisterSecondStepModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterSecondStepHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(RegisterSecondStepModel request, CancellationToken cancellationToken)
        {



            var User = await _userManager.FindByIdAsync(request.UserId);
            if (User is null) return Failed<string>(HttpStatusCode.NotFound, "User id isn't found");

            if (User.Email != request.AuthorizeEmail)
                return Failed<string>(HttpStatusCode.Unauthorized, "you are not Account owner");

            User.FirstName = request.FirstName;
            User.LastName = request.LastName;
            User.PhoneNumber = request.PhoneNumber;
            User.BOD = request.BOD;
            User.Location = request.Location;
            User.Gender = request.Gender;

            if (request.ProfilePicture is not null)
            {
                var PicUrl = DocumentServices.UploadFile(request.ProfilePicture, "ProfileImages", _httpContextAccessor);
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

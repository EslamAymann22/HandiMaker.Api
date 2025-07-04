using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Command
{
    public class RegisterDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }

    public class RegisterModel : IRequest<BaseResponse<RegisterDto>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserRole Role { get; set; }
    }


    public class RegisterHandler : BaseResponseHandler, IRequestHandler<RegisterModel, BaseResponse<RegisterDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthenticationServices _authenticationServices;

        public RegisterHandler(UserManager<AppUser> userManager, IAuthenticationServices authenticationServices)
        {
            _userManager = userManager;
            _authenticationServices = authenticationServices;
        }
        public async Task<BaseResponse<RegisterDto>> Handle(RegisterModel request, CancellationToken cancellationToken)
        {

            if (request.Password != request.ConfirmPassword)
                return Failed<RegisterDto>(HttpStatusCode.BadRequest, "Password Not Match with ConfirmPassword");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
                return Failed<RegisterDto>(HttpStatusCode.Conflict, "This Email is used before!");

            user = await _userManager.FindByNameAsync(request.UserName);
            if (user is not null)
                return Failed<RegisterDto>(HttpStatusCode.Conflict, "This UserName is used before!");

            if (request.Role < UserRole.Customer || request.Role > UserRole.Blocked)
                return Failed<RegisterDto>(HttpStatusCode.NotFound, "This role is not found");

            user = new()
            {
                UserName = request.UserName,
                FirstName = request.UserName,
                Email = request.Email,
                Role = request.Role
            };

            var Result = await _userManager.CreateAsync(user, request.Password);
            if (Result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(request.Email);
                return Success(new RegisterDto()
                {
                    UserId = user.Id,
                    Token = await _authenticationServices.GetJWTTokenAsync(user, _userManager)
                });
            }
            return Failed<RegisterDto>(HttpStatusCode.InternalServerError);
        }
    }

}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Command
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string PictureUrl { get; set; }
        public UserRole Role { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }


    public class LoginModel : IRequest<BaseResponse<LoginDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class LoginHandler : BaseResponseHandler, IRequestHandler<LoginModel, BaseResponse<LoginDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthenticationServices _authenticationServices;

        public LoginHandler(UserManager<AppUser> userManager, IAuthenticationServices authenticationServices)
        {
            _userManager = userManager;
            _authenticationServices = authenticationServices;
        }

        public async Task<BaseResponse<LoginDto>> Handle(LoginModel request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                return Failed<LoginDto>(HttpStatusCode.NotFound, "Email or password is wrong");

            var Result = new LoginDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PictureUrl = user.PictureUrl,
                Role = user.Role,
                UserName = user.UserName,
                Token = await _authenticationServices.GetJWTTokenAsync(user, _userManager)
            };


            return Success(Result);


        }
    }

}

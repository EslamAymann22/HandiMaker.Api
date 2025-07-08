using HandiMaker.Core.Feature.Account.Command;
using HandiMaker.Core.Feature.Account.Command.ForgetPassword;
using HandiMaker.Core.Feature.Account.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse<RegisterDto>>> Register([FromBody] RegisterModel model)
        {
            return BaseOk(await _mediator.Send(model));
        }
        [Authorize]
        [HttpPatch("RegisterSecondStep")]
        public async Task<ActionResult<BaseResponse<string>>> RegisterSecondStep([FromForm] RegisterSecondStepModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<LoginDto>>> Login([FromBody] LoginModel model)
        {
            return BaseOk(await _mediator.Send(model));
        }
        [Authorize]
        [HttpPut("EditProfile")]
        public async Task<ActionResult<BaseResponse<string>>> EditProfile([FromForm] EditAccountModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<BaseResponse<GetUserByIdDto>>> GetUserById([FromQuery] GetUserByIdModel model)
        {

            model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }


        [Authorize]
        [HttpGet("GetCurUser")]
        public async Task<ActionResult<BaseResponse<GetCurUserDto>>> GetCurUser()
        {
            var model = new GetCurUserModel() { UserEmail = User.FindFirstValue(ClaimTypes.Email) };
            return BaseOk(await _mediator.Send(model));
        }


        [HttpPost("RequestResetPass")]
        public async Task<ActionResult<BaseResponse<string>>> RequestResetPass([FromBody] RequestResetPassModel model)
        {
            return BaseOk(await _mediator.Send(model));
        }

        [HttpPost("CheckOTPCode")]
        public async Task<ActionResult<BaseResponse<string>>> CheckOTPCode([FromBody] CheckOTPCodeModel model)
        {
            return BaseOk(await _mediator.Send(model));
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<BaseResponse<string>>> ResetPassword(ResetPasswordModel model)
        {
            return BaseOk(await _mediator.Send(model));
        }
        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<ActionResult<BaseResponse<string>>> ChangePassword([FromBody] ChangePasswordModel model)
        {
            model.AuthEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }
    }
}

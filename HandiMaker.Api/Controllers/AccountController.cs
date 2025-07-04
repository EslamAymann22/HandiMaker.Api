using HandiMaker.Core.Feature.Account.Command;
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
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<LoginDto>>> Login([FromBody] LoginModel model)
        {
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

    }
}

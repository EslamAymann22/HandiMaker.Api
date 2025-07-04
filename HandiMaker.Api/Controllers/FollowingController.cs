using HandiMaker.Core.Feature.Followers.Command;
using HandiMaker.Core.Feature.Followers.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{

    public class FollowingController : BaseController
    {
        private readonly IMediator _mediator;

        public FollowingController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize]
        [HttpPost("FollowUser")]
        public async Task<ActionResult<BaseResponse<string>>> FollowUser([FromQuery] FollowUserModel model)
        {
            model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }
        [Authorize]
        [HttpDelete("UnFollowUser")]
        public async Task<ActionResult<BaseResponse<string>>> UnFollowUser([FromQuery] UnFollowUserModel model)
        {
            model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetUSerFollowers")]
        public async Task<ActionResult<PaginatedResponse<GetUserFollowersAndFollowingDto>>> GetUSerFollowers([FromQuery] GetUserFollowersAndFollowingModel Model)
        {
            Model.NeedFollowers = true;
            Model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _mediator.Send(Model));
        }

        [HttpGet("GetUSerFollowing")]
        public async Task<ActionResult<PaginatedResponse<GetUserFollowersAndFollowingDto>>> GetUSerFollowing([FromQuery] GetUserFollowersAndFollowingModel Model)
        {
            Model.NeedFollowers = false;
            Model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _mediator.Send(Model));
        }

    }
}

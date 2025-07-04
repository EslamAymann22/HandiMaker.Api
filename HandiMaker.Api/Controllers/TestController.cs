using HandiMaker.Core.Feature.Followers.Query;
using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("GetAllFollowers")]
        public async Task<ActionResult<PaginatedResponse<GetUserFollowersAndFollowingDto>>> GetUserFollowers([FromQuery] GetUserFollowersAndFollowingModel Model)
        {
            Model.RequestUserEmail = User.FindFirstValue(ClaimTypes.Email);
            Model.NeedFollowers = true;
            return Ok(await _mediator.Send(Model));
        }



    }
}

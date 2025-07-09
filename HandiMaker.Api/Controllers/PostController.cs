using HandiMaker.Core.Feature.Post.Command;
using HandiMaker.Core.Feature.Post.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{

    public class PostController : BaseController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<ActionResult<BaseResponse<string>>> CreatePost([FromForm] CreatePostModel model)
        {
            model.AuthorEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [Authorize]
        [HttpDelete("DeletePost")]
        public async Task<ActionResult<BaseResponse<string>>> DeletePost([FromQuery] DeletePostModel model)
        {
            model.AuthorEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [Authorize]
        [HttpPut("EditPost")]
        public async Task<ActionResult<BaseResponse<string>>> EditPost([FromForm] EditPostModel model)
        {
            model.AuthorEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetAllUserPosts")]
        public async Task<ActionResult<BaseResponse<List<GetAllUserPostsDto>>>> GetAllUserPosts([FromQuery] GetAllUserPostsModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));

        }
    }
}

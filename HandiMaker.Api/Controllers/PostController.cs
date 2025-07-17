using HandiMaker.Core.Feature.Post.Command;
using HandiMaker.Core.Feature.Post.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Core.ResponseBase.Paginations;
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
        public async Task<ActionResult<BaseResponse<List<GetPostDto>>>> GetAllUserPosts([FromQuery] GetAllUserPostsModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetPostById")]
        public async Task<ActionResult<BaseResponse<GetPostDto>>> GetPostById([FromQuery] GetPostByIdModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetHomePosts")]
        public async Task<ActionResult<PaginatedResponse<GetHomePostDto>>> GetHomePosts([FromQuery] GetHomePostsModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return await _mediator.Send(model);
        }


    }
}

using HandiMaker.Core.Feature.Comments.Command;
using HandiMaker.Core.Feature.Comments.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{

    public class CommentController : BaseController
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("CommentAtPost")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> CommentAtPost([FromBody] AddCommentModel Model)
        {
            Model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(Model));
        }
        [HttpDelete("DeleteComment")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> DeleteComment([FromQuery] DeleteCommentModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }


        [HttpGet("GetAllPostComments")]
        public async Task<PaginatedResponse<GetCommentDto>> GetAllPostComments([FromQuery] GetPostCommentsModel model)
        {
            return await _mediator.Send(model);
        }

        [HttpGet("ShowCommentReplay")]
        public async Task<ActionResult<PaginatedResponse<GetCommentDto>>> ShowCommentReplay([FromQuery] ShowCommentReplayModel model)
        {
            return await _mediator.Send(model);
        }

    }
}

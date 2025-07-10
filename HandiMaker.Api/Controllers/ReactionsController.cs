using HandiMaker.Core.Feature.Reacts.Command;
using HandiMaker.Core.Feature.Reacts.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{
    public class ReactionsController : BaseController
    {
        private readonly IMediator _mediator;

        public ReactionsController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [Authorize]
        [HttpPost("AddReaction")]
        public async Task<ActionResult<BaseResponse<string>>> AddReaction([FromQuery] ChangeReactionModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            model.AddReaction = true;
            return BaseOk(await _mediator.Send(model));
        }
        [Authorize]
        [HttpDelete("DeleteReaction")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteReaction([FromQuery] ChangeReactionModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            model.AddReaction = false;
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetAllReactions")]
        public async Task<ActionResult<BaseResponse<List<string>>>> GetAllReactions([FromQuery] GetAllReactionsModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));


        }
    }
}

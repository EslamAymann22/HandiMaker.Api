using HandiMaker.Core.Feature.Chat.Command;
using HandiMaker.Core.Feature.Chat.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageModel Model)
        {
            Model.FromId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return BaseOk(await _mediator.Send(Model));
        }

        [HttpGet("GetAllChats")]
        public async Task<ActionResult<GetAllUserChatDto>> GetAllChats([FromQuery] GetAllUserChatModel Model)
        {
            Model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _mediator.Send(Model));
        }
        [HttpPut("MarkSeen")]
        public async Task<IActionResult> MarkSeen([FromQuery] MarkMessageSeenModel Model)
        {
            Model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return BaseOk(await _mediator.Send(Model));
        }

        [HttpGet("GetChatMessages")]
        public async Task<ActionResult<GetChatMessagesDto>> GetChatMessages([FromQuery] GetChatMessagesModel Model)
        {
            Model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _mediator.Send(Model));
        }

    }
}

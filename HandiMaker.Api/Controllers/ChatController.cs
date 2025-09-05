using HandiMaker.Core.Feature.Chat.Command;
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

    }
}

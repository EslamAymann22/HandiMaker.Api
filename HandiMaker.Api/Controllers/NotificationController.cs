using HandiMaker.Core.Feature.Notificaion.Command;
using HandiMaker.Core.Feature.Notificaion.Query;
using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet("GetUserNotifications")]
        public async Task<ActionResult<PaginatedResponse<GetUserNotificationsDto>>> GetUserNotifications([FromQuery] GetUserNotificationsModel model)
        {
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("MakeNotifiAsRead")]
        public async Task<ActionResult> MakeNotifiAsRead([FromQuery] MakeNotifiAsReadModel model)
        {
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return BaseOk(await _mediator.Send(model));
        }

    }
}

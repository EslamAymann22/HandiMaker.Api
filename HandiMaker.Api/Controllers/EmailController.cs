using HandiMaker.Api.Controllers;
using HandiMaker.Core.Feature.Email.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HandiMaker.APIs.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]  
    public class EmailsController : BaseController
    {

        private readonly IMediator _mediator;

        public EmailsController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailModel command)
        {
            var response = await _mediator.Send(command);
            return BaseOk(response);
        }
    }
}

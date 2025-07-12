using HandiMaker.Core.Feature.Products.Command;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HandiMaker.Api.Controllers
{

    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "HandiMaker")]
        public async Task<ActionResult<BaseResponse<string>>> CreateProduct([FromForm] CreateProductModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }

        [HttpPut("EditProduct")]
        [Authorize(Roles = "HandiMaker")]
        public async Task<ActionResult<BaseResponse<string>>> EditProduct([FromForm] EditProductModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return BaseOk(await _mediator.Send(model));
        }
    }
}

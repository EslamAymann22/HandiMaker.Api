using HandiMaker.Core.Feature.Products.Command;
using HandiMaker.Core.Feature.Products.Query;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Core.ResponseBase.Paginations;
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

        [HttpPost("AddProductToFav")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> AddProductToFav([FromQuery] ChangeFavProductStatusModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            model.AddToFav = true;
            return BaseOk(await _mediator.Send(model));
        }

        [HttpDelete("RemoveProductFromFav")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> RemoveProductFromFav([FromQuery] ChangeFavProductStatusModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            model.AddToFav = false;
            return BaseOk(await _mediator.Send(model));
        }

        [HttpGet("GetAllUserProducts")]
        public async Task<ActionResult<PaginatedResponse<GetProductsDto>>> GetAllUserProducts([FromQuery] GetAllUserProductsModel model)
        {
            model.AuthorizeEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _mediator.Send(model));
        }

    }
}

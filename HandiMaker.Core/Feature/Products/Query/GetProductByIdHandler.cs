using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Products.Query
{
    public class GetProductByIdModel : IRequest<BaseResponse<GetProductsDto>>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdHandler : BaseResponseHandler, IRequestHandler<GetProductByIdModel, BaseResponse<GetProductsDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetProductByIdHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<BaseResponse<GetProductsDto>> Handle(GetProductByIdModel request, CancellationToken cancellationToken)
        {
            var product = await _handiMakerDb.Products
                .Where(p => p.Id == request.ProductId)
                .Include(p => p.Owner)
                .Include(p => p.ProductPictures)
                .Select(P => new GetProductsDto
                {
                    ProductId = P.Id,
                    Title = P.Title,
                    Description = P.Description,
                    Category = P.Category,
                    Type = P.Type,
                    OwnerImageUrl = P.Owner.PictureUrl,
                    OwnerName = P.Owner.FirstName + " " + P.Owner.LastName,
                    DeliveryAt = P.DeliveryAt,
                    ProductImages = P.ProductPictures.Select(PP => PP.PictureUrl).ToList(),
                })
                .FirstOrDefaultAsync();

            if (product is null)
                return Failed<GetProductsDto>(HttpStatusCode.NotFound, "Product not found");

            return Success(product);
        }
    }

}

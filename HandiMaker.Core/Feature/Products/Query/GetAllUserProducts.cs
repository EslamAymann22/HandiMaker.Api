using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Products.Query
{

    public class GetProductsDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateOnly DeliveryAt { get; set; }

        public string? OwnerImageUrl { get; set; }
        public string OwnerName { get; set; }
        public List<string> ProductImages { get; set; } = new();
        public bool IsFav { get; set; } = false;

    }

    public class GetAllUserProductsModel : PaginationParams, IRequest<PaginatedResponse<GetProductsDto>>
    {
        public string UserId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }
    public class GetAllUserProductsHandler : IRequestHandler<GetAllUserProductsModel, PaginatedResponse<GetProductsDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetAllUserProductsHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<PaginatedResponse<GetProductsDto>> Handle(GetAllUserProductsModel request, CancellationToken cancellationToken)
        {
            var HamdiMakerUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == request.UserId);

            if (HamdiMakerUser is null)
                return null;

            var RequesUser = await _handiMakerDb.Users
                .Include(U => U.FavProducts).FirstOrDefaultAsync(U => U.Email == request.AuthorizeEmail);

            var FavProducts = RequesUser?.FavProducts.Select(FP => FP.Id).ToList() ?? new();

            var QData = _handiMakerDb.Products.Where(P => P.OwnerId == request.UserId)
                .Include(P => P.ProductPictures)
                .Select(P => new GetProductsDto
                {
                    ProductId = P.Id,
                    Title = P.Title,
                    Description = P.Description,
                    Category = P.Category,
                    Type = P.Type,
                    DeliveryAt = P.DeliveryAt,
                    OwnerImageUrl = HamdiMakerUser.PictureUrl,
                    OwnerName = HamdiMakerUser.FirstName + " " + HamdiMakerUser.LastName,
                    ProductImages = P.ProductPictures.Select(PP => PP.PictureUrl).ToList(),
                    IsFav = FavProducts.Any(PId => PId == P.Id)
                });
            var ResultData = await QData.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return ResultData;

        }
    }


}




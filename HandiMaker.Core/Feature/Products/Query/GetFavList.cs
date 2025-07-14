using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Products.Query
{
    public class GetFavListModel : PaginationParams, IRequest<PaginatedResponse<GetProductsDto>>
    {
        public string? AuthorizeEmail { get; set; }
    }

    public class GetFavListHandler : IRequestHandler<GetFavListModel, PaginatedResponse<GetProductsDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetFavListHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<PaginatedResponse<GetProductsDto>> Handle(GetFavListModel request, CancellationToken cancellationToken)
        {
            var user = _handiMakerDb.Users.FirstOrDefault(U => U.Email == request.AuthorizeEmail);
            if (user is null)
                throw new Exception("User not found");

            var Qdata = _handiMakerDb.Users
                .Where(U => U.Id == user.Id)
                .Include(U => U.FavProducts)
                .ThenInclude(P => P.Owner)
                .Include(U => U.FavProducts)
                .ThenInclude(P => P.ProductPictures)
                .SelectMany(U => U.FavProducts)
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
                    IsFav = true
                });

            return await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}

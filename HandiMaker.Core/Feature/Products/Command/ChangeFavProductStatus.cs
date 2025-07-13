using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Products.Command
{
    public class ChangeFavProductStatusModel : IRequest<BaseResponse<string>>
    {
        public int ProductId { get; set; }
        public string? AuthorizeEmail { get; set; }
        public bool? AddToFav { get; set; } = true;
    }

    public class ChangeFavProductStatusHandler : BaseResponseHandler, IRequestHandler<ChangeFavProductStatusModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public ChangeFavProductStatusHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<string>> Handle(ChangeFavProductStatusModel request, CancellationToken cancellationToken)
        {
            var Product = await _handiMakerDb.Products.FirstOrDefaultAsync(P => P.Id == request.ProductId);

            if (Product is null)
                return Failed<string>(HttpStatusCode.NotFound, "this product is not found");

            var user = await _handiMakerDb.Users.Include(U => U.FavProducts).FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));

            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized);

            var AlreadyAdded = user.FavProducts.Any(P => P.Id == request.ProductId);
            if (request.AddToFav.Value)
            {
                if (AlreadyAdded)
                    return Failed<string>(HttpStatusCode.BadRequest, "Product Already at fav List");
                user.FavProducts.Add(Product);
            }
            else
            {
                if (!AlreadyAdded)
                    return Failed<string>(HttpStatusCode.BadRequest, "Product not found in fav List");
                user.FavProducts.Remove(Product);
            }

            try
            {
                _handiMakerDb.Update(user);
                await _handiMakerDb.SaveChangesAsync();
                return Success((request.AddToFav.Value ? "Added" : "Removed") + "Successfully");
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }


}

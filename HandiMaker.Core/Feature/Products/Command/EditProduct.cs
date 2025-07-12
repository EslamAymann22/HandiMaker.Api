using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Products.Command
{
    public class EditProductModel : CreateProductModel, IRequest<BaseResponse<string>>
    {
        public int ProductId { get; set; }
    }
    public class EditProductHandler : BaseResponseHandler, IRequestHandler<EditProductModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditProductHandler(HandiMakerDbContext handiMakerDb, IHttpContextAccessor httpContextAccessor)
        {
            this._handiMakerDb = handiMakerDb;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(EditProductModel request, CancellationToken cancellationToken)
        {
            var product = await _handiMakerDb.Products
                .Include(P => P.ProductPictures)
                .Include(P => P.ProductColors)
                .FirstOrDefaultAsync(P => P.Id == request.ProductId);
            if (product is null)
                return Failed<string>(HttpStatusCode.NotFound, "This product is not found");
            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == (request.AuthorizeEmail ?? ""));

            if (user is null || user.Id != product.OwnerId)
                return Failed<string>(HttpStatusCode.Unauthorized, "Can't Edit this Product");

            product.ProductPictures.Clear();
            product.ProductColors.Clear();

            foreach (var pic in request.ProductPictures)
            {
                var PicUrl = DocumentServices.UploadFile(pic, FoldersName.ProductImages.ToString(), _httpContextAccessor);
                product.ProductPictures.Add(new() { PictureUrl = PicUrl });
            }
            foreach (var color in request.ProductColors)
                product.ProductColors.Add(new() { Color = color });

            try
            {

                _handiMakerDb.Products.Update(product);
                await _handiMakerDb.SaveChangesAsync();
                return Success("Product Edited !!");
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

    }


}

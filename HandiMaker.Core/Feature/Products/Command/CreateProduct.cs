using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities.ProductClasses;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Products.Command
{
    public class CreateProductModel : IRequest<BaseResponse<string>>
    {
        public string? AuthorizeEmail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public DateOnly DeliveryAt { get; set; }
        public List<int>? ProductColors { get; set; } = new();
        public List<IFormFile>? ProductPictures { get; set; } = new();
    }

    public class CreateProductHandler : BaseResponseHandler, IRequestHandler<CreateProductModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateProductHandler(HandiMakerDbContext handiMakerDb, IHttpContextAccessor httpContextAccessor)
        {
            this._handiMakerDb = handiMakerDb;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(CreateProductModel request, CancellationToken cancellationToken)
        {
            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));
            if (user is null || user.Role != UserRole.HandiMaker)
                return Failed<string>(HttpStatusCode.Unauthorized, "Must be HandiMaker");

            var product = new Product
            {
                Title = request.Title,
                Category = request.Category,
                DeliveryAt = request.DeliveryAt,
                Description = request.Description,
                OwnerId = user.Id,
                Type = request.Type,
                Size = request.Size,
                ProductColors = new(),
                ProductPictures = new()
            };

            foreach (var pic in request.ProductPictures)
            {
                var PicUrl = DocumentServices.UploadFile(pic, FoldersName.ProductImages.ToString(), _httpContextAccessor);
                product.ProductPictures.Add(new() { PictureUrl = PicUrl });
            }
            foreach (var color in request.ProductColors)
                product.ProductColors.Add(new() { Color = color });

            try
            {

                _handiMakerDb.Products.Add(product);
                await _handiMakerDb.SaveChangesAsync();
                return Success("Product added !!");
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, ex.Message);
            }


        }
    }

}

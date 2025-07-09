using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HandiMaker.Core.Feature.Post.Command
{
    public class CreatePostModel : IRequest<BaseResponse<string>>
    {

        public string? Content { get; set; } = "";
        public List<IFormFile>? Pictures { get; set; } = new();
        public string? AuthorEmail { get; set; }
    }
    public class CreatePostHandler : BaseResponseHandler, IRequestHandler<CreatePostModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatePostHandler(HandiMakerDbContext handiMakerDb, IHttpContextAccessor httpContextAccessor)
        {
            this._handiMakerDb = handiMakerDb;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(CreatePostModel request, CancellationToken cancellationToken)
        {
            var User = _handiMakerDb.Users.FirstOrDefault(u => u.Email == request.AuthorEmail);
            if (User is null) return Failed<string>(System.Net.HttpStatusCode.Unauthorized);

            var post = new Data.Entities.PostClasses.Post
            {
                Content = request.Content,
                PostOwnerId = User.Id,
                CreatedAt = DateTime.UtcNow,
                postPictures = new()
            };

            foreach (var picture in request.Pictures ?? new())
            {
                var picUrl = DocumentServices.UploadFile(picture, FoldersName.PostImages.ToString(), _httpContextAccessor);
                if (picUrl is not null)
                    post.postPictures.Add(new() { PicturUrl = picUrl });
            }

            _handiMakerDb.Posts.Add(post);
            var result = await _handiMakerDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Post created successfully");

            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to create post");
        }
    }


}

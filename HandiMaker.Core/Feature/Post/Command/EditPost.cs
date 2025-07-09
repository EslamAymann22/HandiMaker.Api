using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.HelperStatic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Post.Command
{
    public class EditPostModel : IRequest<BaseResponse<string>>
    {
        public int PostId { get; set; }
        public string? AuthorEmail { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? Pictures { get; set; } = new();
    }


    public class EditPostHandler : BaseResponseHandler, IRequestHandler<EditPostModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditPostHandler(HandiMakerDbContext handiMakerDb, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._handiMakerDb = handiMakerDb;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(EditPostModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.AuthorEmail);
            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized);
            var post = await _handiMakerDb.Posts.Include(P => P.postPictures).FirstOrDefaultAsync(P => P.Id == request.PostId);
            if (post is null)
                return Failed<string>(HttpStatusCode.NotFound, "Post not found");
            if (post.PostOwnerId != user.Id)
                return Failed<string>(HttpStatusCode.Forbidden, "You are not allowed to delete this post");

            post.Content = request.Content ?? post.Content;
            post.postPictures.Clear();
            foreach (var pic in request.Pictures ?? new())
            {
                var PicUrl = DocumentServices.UploadFile(pic, FoldersName.PostImages.ToString(), _httpContextAccessor);
                if (PicUrl is not null)
                    post.postPictures.Add(new() { PicturUrl = PicUrl });
            }
            _handiMakerDb.Posts.Update(post);
            var result = await _handiMakerDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Post edited successfully");
            return Failed<string>(HttpStatusCode.InternalServerError, "Failed to edit post");

        }
    }
}

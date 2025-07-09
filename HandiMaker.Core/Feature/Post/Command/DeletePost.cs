using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HandiMaker.Core.Feature.Post.Command
{
    public class DeletePostModel : IRequest<BaseResponse<string>>
    {

        public int PostId { get; set; }
        public string? AuthorEmail { get; set; }

    }


    public class DeletePostHandler : BaseResponseHandler, IRequestHandler<DeletePostModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly UserManager<AppUser> _userManager;

        public DeletePostHandler(HandiMakerDbContext handiMakerDb, UserManager<AppUser> userManager)
        {
            this._handiMakerDb = handiMakerDb;
            this._userManager = userManager;
        }

        public async Task<BaseResponse<string>> Handle(DeletePostModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.AuthorEmail);
            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized);
            var post = await _handiMakerDb.Posts.FindAsync(request.PostId);
            if (post is null)
                return Failed<string>(HttpStatusCode.NotFound, "Post not found");
            if (post.PostOwnerId != user.Id)
                return Failed<string>(HttpStatusCode.Forbidden, "You are not allowed to delete this post");

            _handiMakerDb.Posts.Remove(post);
            var result = await _handiMakerDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Post deleted successfully");
            return Failed<string>(HttpStatusCode.InternalServerError, "Failed to delete post");
        }
    }
}

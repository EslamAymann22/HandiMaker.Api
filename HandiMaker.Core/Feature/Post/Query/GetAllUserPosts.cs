using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Post.Query
{
    public class GetPostDto
    {
        public int PostId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> PostPictures { get; set; } = new();
        public int numberOfLikes { get; set; }
        public int numberOfComments { get; set; }
        public bool LoveIt { get; set; } = false;
    }
    public class GetAllUserPostsModel : IRequest<BaseResponse<List<GetPostDto>>>
    {
        public string UserId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }

    public class GetAllUserPostsHandler : BaseResponseHandler, IRequestHandler<GetAllUserPostsModel, BaseResponse<List<GetPostDto>>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly UserManager<AppUser> _userManager;

        public GetAllUserPostsHandler(HandiMakerDbContext handiMakerDb, UserManager<AppUser> userManager)
        {
            this._handiMakerDb = handiMakerDb;
            this._userManager = userManager;
        }

        public async Task<BaseResponse<List<GetPostDto>>> Handle(GetAllUserPostsModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Failed<List<GetPostDto>>(HttpStatusCode.NotFound, "User not found");

            var AuthorizedUser = await _userManager.FindByEmailAsync(request.AuthorizeEmail ?? "");

            var PostsQ = _handiMakerDb.Posts.Where(P => P.PostOwnerId == user.Id)
                .Include(P => P.ReactedUsers).Include(P => P.Comments).Include(P => P.PostOwner).Include(P => P.postPictures);
            var posts = await PostsQ.Select(P => new GetPostDto
            {
                PostId = P.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Content = P.Content,
                ImageUrl = P.PostOwner.PictureUrl,
                CreatedAt = P.CreatedAt,
                PostPictures = P.postPictures.Select(PP => PP.PicturUrl).ToList(),
                numberOfLikes = P.ReactedUsers.Count,
                numberOfComments = P.Comments.Count,
                LoveIt = AuthorizedUser != null && P.ReactedUsers.Any(RU => RU.Id == AuthorizedUser.Id)
            }).ToListAsync(cancellationToken);

            return Success(posts);

        }
    }
}

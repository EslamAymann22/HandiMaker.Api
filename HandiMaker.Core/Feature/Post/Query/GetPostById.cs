using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Post.Query
{
    public class GetPostByIdModel : IRequest<BaseResponse<GetPostDto>>
    {
        public int postId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }
    public class GetPostByIdHandler : BaseResponseHandler, IRequestHandler<GetPostByIdModel, BaseResponse<GetPostDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly UserManager<AppUser> _userManager;

        public GetPostByIdHandler(HandiMakerDbContext handiMakerDb, UserManager<AppUser> userManager)
        {
            this._handiMakerDb = handiMakerDb;
            this._userManager = userManager;
        }

        public async Task<BaseResponse<GetPostDto>> Handle(GetPostByIdModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.AuthorizeEmail ?? "");

            var post = await _handiMakerDb.Posts.Where(P => P.Id == request.postId).Include(P => P.PostOwner).Select(
                P => new GetPostDto
                {
                    PostId = P.Id,
                    FirstName = P.PostOwner.FirstName,
                    LastName = P.PostOwner.LastName,
                    Content = P.Content,
                    ImageUrl = P.PostOwner.PictureUrl,
                    CreatedAt = P.CreatedAt,
                    PostPictures = P.postPictures.Select(PP => PP.PicturUrl).ToList(),
                    numberOfLikes = P.ReactedUsers.Count,
                    numberOfComments = P.Comments.Count,
                    LoveIt = user != null && P.ReactedUsers.Any(RU => RU.Id == user.Id)
                }).FirstOrDefaultAsync();
            //return null;
            return Success(post ?? new());

        }
    }



}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Followers.Command
{
    public class UnFollowUserModel : IRequest<BaseResponse<string>>
    {
        public string UnFollowedUserId { get; set; }
        public string? RequestUserEmail { get; set; }
    }

    public class UnFollowUserHandler : BaseResponseHandler, IRequestHandler<UnFollowUserModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public UnFollowUserHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<BaseResponse<string>> Handle(UnFollowUserModel request, CancellationToken cancellationToken)
        {
            var UnFollowedUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == request.UnFollowedUserId);
            var RequestUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == request.RequestUserEmail);

            if (UnFollowedUser is null)
                return Failed<string>(HttpStatusCode.NotFound, "UnFollowed User Not Found");

            var IsFollowed = await _handiMakerDb.UserFollows
                .FirstOrDefaultAsync(UF => UF.FollowerId == RequestUser.Id && UF.FollowedId == request.UnFollowedUserId);

            if (IsFollowed is null)
                return Failed<string>(HttpStatusCode.BadRequest, $"{RequestUser.UserName} Not following {UnFollowedUser.UserName}");

            _handiMakerDb.UserFollows.Remove(IsFollowed);

            await _handiMakerDb.SaveChangesAsync(cancellationToken);

            return Success($"{RequestUser.UserName} Unfollow {UnFollowedUser.UserName} Now!");
        }
    }

}

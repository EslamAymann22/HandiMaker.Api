using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities.UserClassese;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Followers.Command
{
    public class FollowUserModel : IRequest<BaseResponse<string>>
    {
        public string FollowedUserId { get; set; }
        public string? RequestUserEmail { get; set; }
    }


    public class FollowUserHandler : BaseResponseHandler, IRequestHandler<FollowUserModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public FollowUserHandler(HandiMakerDbContext handiMakerDb)
        {
            _handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<string>> Handle(FollowUserModel request, CancellationToken cancellationToken)
        {
            var FollowedUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == request.FollowedUserId);
            var RequestUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == request.RequestUserEmail);

            if (FollowedUser.Id == RequestUser.Id)
                return Failed<string>(HttpStatusCode.BadRequest, "User Want Follow HimSelf!!");

            var IsFollowed = _handiMakerDb.UserFollows
                .Any(UF => UF.FollowerId == RequestUser.Id && UF.FollowedId == request.FollowedUserId);

            if (IsFollowed)
                return Failed<string>(HttpStatusCode.BadRequest, $"{RequestUser.UserName} Already following {FollowedUser.UserName}");

            _handiMakerDb.UserFollows.Add(new UserFollow
            {
                FollowedAt = DateTime.Now,
                FollowerId = RequestUser.Id,
                FollowedId = request.FollowedUserId
            });

            await _handiMakerDb.SaveChangesAsync(cancellationToken);

            return Success($"{RequestUser.UserName} follow {FollowedUser.UserName} Now!");



        }
    }
}

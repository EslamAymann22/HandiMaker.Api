using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities.UserClassese;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly INotificationServices _notificationServices;

        public FollowUserHandler(HandiMakerDbContext handiMakerDb
            , IConfiguration configuration
            , INotificationServices notificationServices)
        {
            _handiMakerDb = handiMakerDb;
            this._configuration = configuration;
            this._notificationServices = notificationServices;
        }
        public async Task<BaseResponse<string>> Handle(FollowUserModel request, CancellationToken cancellationToken)
        {
            var FollowedUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == request.FollowedUserId);
            var RequestUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == request.RequestUserEmail);
            if (FollowedUser is null)
                return Failed<string>(HttpStatusCode.NotFound, "Followed User Not Found");
            if (RequestUser is null)
                return Failed<string>(HttpStatusCode.Unauthorized, "Request User Not Found or Unauthorized");
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

            try
            {
                var RouteLink = $"{_configuration["BaseUrl"]}/api/Account/GetUserById?UserId={RequestUser.Id}";
                await _notificationServices.SendNotificationAsync(RequestUser,
                    $"{RequestUser.FirstName + " " + RequestUser.LastName} started following you.",
                    FollowedUser.Id,
                    RouteLink, NotifiType.NewFollower);
            }
            catch (Exception ex)
            {
                var Error = ex.Message;
                return Success($"{RequestUser.UserName} follow {FollowedUser.UserName} Now!, But Notification Not Sent =>\n\n{Error}");
            }
            return Success($"{RequestUser.UserName} follow {FollowedUser.UserName} Now!");



        }
    }
}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Query
{
    public class GetCurUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
    }
    public class GetCurUserModel : IRequest<BaseResponse<GetCurUserDto>>
    {
        public string UserEmail { get; set; }
    }

    public class GetCurUserHandler : BaseResponseHandler, IRequestHandler<GetCurUserModel, BaseResponse<GetCurUserDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetCurUserHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<GetCurUserDto>> Handle(GetCurUserModel request, CancellationToken cancellationToken)
        {
            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == request.UserEmail);

            if (user is null)
                return Failed<GetCurUserDto>(HttpStatusCode.NotFound, "User Is Not Found");

            return Success(new GetCurUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PictureUrl = user.PictureUrl
            });

        }
    }
}

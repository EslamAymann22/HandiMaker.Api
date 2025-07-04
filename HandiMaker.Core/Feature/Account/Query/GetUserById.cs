using AutoMapper;
using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Account.Query
{
    public class GetUserByIdDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string PictureUrl { get; set; }
        public int NumberOfFollowers { get; set; }
        public int NumberOfFollowing { get; set; }
        public int NumberOfPosts { get; set; }
        public bool IsFollowed { get; set; } = false;
    }

    public class GetUserByIdModel : IRequest<BaseResponse<GetUserByIdDto>>
    {

        public string UserId { get; set; }
        public string? RequestUserEmail { get; set; }

    }

    public class GetUserByIdHandler : BaseResponseHandler, IRequestHandler<GetUserByIdModel, BaseResponse<GetUserByIdDto>>
    {

        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(HandiMakerDbContext handiMakerDb, IMapper mapper)
        {
            _handiMakerDb = handiMakerDb;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetUserByIdDto>> Handle(GetUserByIdModel request, CancellationToken cancellationToken)
        {
            var user = await _handiMakerDb.Users.Where(U => U.Id == request.UserId).FirstOrDefaultAsync();



            if (user is null)
                return Failed<GetUserByIdDto>(HttpStatusCode.NotFound, "User is not found");

            var Result = _mapper.Map<GetUserByIdDto>(user);

            Result.NumberOfFollowing = _handiMakerDb.Entry(user).Collection(U => U.Following).Query().Count();
            Result.NumberOfFollowers = _handiMakerDb.Entry(user).Collection(U => U.Followers).Query().Count();
            Result.NumberOfPosts = _handiMakerDb.Entry(user).Collection(U => U.CreatedPosts).Query().Count();


            var RequestUserId = _handiMakerDb.Users.Where(U => U.Email == request.RequestUserEmail).FirstOrDefault()?.Id;
            Result.IsFollowed = await _handiMakerDb.Users.Where(u => u.Id == request.UserId)
                            .Select(u => u.Followers.Any(f => f.FollowerId == RequestUserId)).FirstOrDefaultAsync();

            return Success(Result);

        }
    }


}

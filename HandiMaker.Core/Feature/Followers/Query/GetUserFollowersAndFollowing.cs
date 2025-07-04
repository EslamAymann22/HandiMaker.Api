using AutoMapper;
using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Followers.Query
{
    public class GetUserFollowersAndFollowingDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public bool IsFollowed { get; set; }
    }

    public class GetUserFollowersAndFollowingModel : PaginationParams, IRequest<PaginatedResponse<GetUserFollowersAndFollowingDto>>
    {
        public string UserId { get; set; }
        public string? RequestUserEmail { get; set; }
        public bool NeedFollowers { get; set; } = true;
    }


    public class GetUserFollowersAndFollowingHandler : IRequestHandler<GetUserFollowersAndFollowingModel, PaginatedResponse<GetUserFollowersAndFollowingDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IMapper _mapper;

        public GetUserFollowersAndFollowingHandler(HandiMakerDbContext handiMakerDb, IMapper mapper)
        {
            this._handiMakerDb = handiMakerDb;
            this._mapper = mapper;
        }
        public async Task<PaginatedResponse<GetUserFollowersAndFollowingDto>> Handle(GetUserFollowersAndFollowingModel request, CancellationToken cancellationToken)
        {
            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Id == request.UserId);

            if (user is null)
                throw new KeyNotFoundException("User is not found");


            var Qdata = request.NeedFollowers
                ? _handiMakerDb.UserFollows.Where(UF => UF.FollowedId == request.UserId).Include(UF => UF.Follower).OrderByDescending(F => F.FollowedAt).Select(UF => UF.Follower)
                : _handiMakerDb.UserFollows.Where(UF => UF.FollowerId == request.UserId).Include(UF => UF.Followed).OrderByDescending(F => F.FollowedAt).Select(UF => UF.Followed);



            var ResultData = await _mapper.ProjectTo<GetUserFollowersAndFollowingDto>(Qdata)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);


            var RequestUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == request.RequestUserEmail);

            if (RequestUser is not null)
            {

                var RequestUserFollowing = _handiMakerDb.UserFollows.Where(uf => uf.FollowerId == RequestUser.Id);

                foreach (var Follow in ResultData.PaginatedData)
                {
                    Follow.IsFollowed = RequestUserFollowing.Any(UF => UF.FollowedId == Follow.Id);
                }
            }

            return ResultData;


        }
    }

}

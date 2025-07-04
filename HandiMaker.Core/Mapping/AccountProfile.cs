using AutoMapper;
using HandiMaker.Core.Feature.Account.Query;
using HandiMaker.Core.Feature.Followers.Query;
using HandiMaker.Data.Entities;

namespace HandiMaker.Core.Mapping
{
    public class AccountProfile : Profile
    {

        public AccountProfile()
        {
            CreateMap<AppUser, GetUserByIdDto>();
            CreateMap<AppUser, GetUserFollowersAndFollowingDto>();

        }

    }
}

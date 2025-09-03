using AutoMapper;
using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HandiMaker.Core.Feature.Notificaion.Query
{

    public class GetUserNotificationsDto
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public DateTime NoteAt { get; set; }
        public string? NotifiedUserPictureUrl { get; set; }
        public string? NotifiedUserFullName { get; set; }
        public string? NotifiedUserId { get; set; }
        public string Content { get; set; }
        public string NotifiType { get; set; }
        public string RouteLink { get; set; }
    }
    public class GetUserNotificationsModel : PaginationParams, IRequest<PaginatedResponse<GetUserNotificationsDto>>
    {
        public bool onlyUnread { get; set; } = false;
        public string? UserId { get; set; }
    }

    public class GetUserNotificationsHandler : IRequestHandler<GetUserNotificationsModel, PaginatedResponse<GetUserNotificationsDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IMapper _mapper;

        public GetUserNotificationsHandler(UserManager<AppUser> userManager
            , HandiMakerDbContext handiMakerDb
            , IMapper mapper)
        {
            this._userManager = userManager;
            this._handiMakerDb = handiMakerDb;
            this._mapper = mapper;
        }

        public async Task<PaginatedResponse<GetUserNotificationsDto>> Handle(GetUserNotificationsModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId ?? "");

            if (user == null)
                throw new Exception("User not found");


            var notificationsQ = _handiMakerDb.Notifications.Where(N => N.UserId == request.UserId).OrderByDescending(N => N.Id).AsQueryable();
            if (request.onlyUnread)
                notificationsQ = notificationsQ.Where(N => N.IsRead == false);
            var MappedData = await _mapper.ProjectTo<GetUserNotificationsDto>(notificationsQ).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            foreach (var noti in MappedData.PaginatedData)
            {
                var nofiedUser = await _userManager.FindByIdAsync(noti.NotifiedUserId ?? "");
                noti.NotifiedUserFullName = nofiedUser?.FirstName + " " + nofiedUser?.LastName;
                noti.NotifiedUserPictureUrl = nofiedUser?.PictureUrl;
            }

            return MappedData;
        }
    }


}

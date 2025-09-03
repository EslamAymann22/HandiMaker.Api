using AutoMapper;
using HandiMaker.Core.Feature.Notificaion.Query;
using HandiMaker.Data.Entities;

namespace HandiMaker.Core.Mapping
{
    public class NotificationsProfile : Profile
    {

        public NotificationsProfile()
        {
            CreateMap<Notification, GetUserNotificationsDto>();

        }

    }
}

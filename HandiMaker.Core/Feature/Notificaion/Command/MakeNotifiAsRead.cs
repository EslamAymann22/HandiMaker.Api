using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;

namespace HandiMaker.Core.Feature.Notificaion.Command
{
    public class MakeNotifiAsReadModel : IRequest<BaseResponse<string>>
    {
        public string? UserId { get; set; }
        public int NotifiId { get; set; }
    }

    public class MakeNotifiAsReadHandler : BaseResponseHandler, IRequestHandler<MakeNotifiAsReadModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public MakeNotifiAsReadHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<string>> Handle(MakeNotifiAsReadModel request, CancellationToken cancellationToken)
        {
            var Notification = _handiMakerDb.Notifications.FirstOrDefault(N => N.Id == request.NotifiId);
            if (Notification == null)
                return NotFound<string>("Notification not found");
            if (Notification.UserId != request.UserId)
                return Failed<string>(System.Net.HttpStatusCode.Forbidden, "You are not allowed to mark this notification as read");
            Notification.IsRead = true;
            await _handiMakerDb.SaveChangesAsync();

            return Success("Notification marked as read successfully");
        }
    }

}

using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Chat.Command
{
    public class MarkMessageSeenModel : IRequest<BaseResponse<string>>
    {
        public string? UserId { get; set; }
        public int MessageId { get; set; }
    }

    public class MarkMessageSeenHandler : BaseResponseHandler, IRequestHandler<MarkMessageSeenModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public MarkMessageSeenHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<string>> Handle(MarkMessageSeenModel request, CancellationToken cancellationToken)
        {
            var message = await _handiMakerDb.Messages.Where(M => M.Id == request.MessageId).Include(M => M.Chat).FirstOrDefaultAsync();

            if (request.UserId is null)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authrize");
            if (message is null)
                return Failed<string>(HttpStatusCode.NotFound, "this message is not found");
            if (message.UserId == request.UserId)
                return Failed<string>(HttpStatusCode.Forbidden, "Can't make this message seen");

            if (!message.IsSeen)
            {
                message.IsSeen = true;
                message.Chat.NumberOfMessagesUnseen--;
                message.Chat.NumberOfMessagesUnseen = Math.Max(0, message.Chat.NumberOfMessagesUnseen);
            }
            else
                return Failed<string>(HttpStatusCode.BadRequest, "Message already seened");
            await _handiMakerDb.SaveChangesAsync();

            return Success("message marked seen successfully");
        }
    }


}

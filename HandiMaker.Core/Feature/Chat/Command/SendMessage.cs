using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HandiMaker.Core.Feature.Chat.Command
{


    public class SendMessageModel : IRequest<BaseResponse<string>>
    {
        public string? FromId { get; set; }
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
        public string ToId { get; set; }
    }

    public class SendMessageHandler : BaseResponseHandler, IRequestHandler<SendMessageModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IChatServices _chatServices;

        public SendMessageHandler(HandiMakerDbContext handiMakerDb, IChatServices chatServices)
        {
            this._handiMakerDb = handiMakerDb;
            this._chatServices = chatServices;
        }
        public async Task<BaseResponse<string>> Handle(SendMessageModel request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Content) && (request.File is null))
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "Message mustn't be empty");

            var message = await _chatServices.SendMessageAsync(request.FromId, request.ToId, request.Content, request.File);

            if (message is null)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "Send message failed");
            return Success("Send message successfully");
        }
    }
}


using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Services.Services.Interface;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HandiMaker.Core.Feature.Email.Command
{
    public class SendEmailModel : IRequest<BaseResponse<string>>
    {
        [EmailAddress]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class SendEmailHandler : BaseResponseHandler,
                                    IRequestHandler<SendEmailModel, BaseResponse<string>>
    {
        private readonly IEmailService _emailService;

        public SendEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task<BaseResponse<string>> Handle(SendEmailModel request, CancellationToken cancellationToken)
        {
            var res = await _emailService.SendEmail(request.ToEmail, request.Subject, request.Body);
            if (res.IsSucceeded)
                return Success("Send Email Successfully");
            else
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, res.MessageResult);
        }
    }

}

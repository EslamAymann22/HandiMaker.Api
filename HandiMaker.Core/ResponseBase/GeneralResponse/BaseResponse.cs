using System.Net;

namespace HandiMaker.Core.ResponseBase.GeneralResponse
{
    public class BaseResponse<T>
    {

        public HttpStatusCode Status { get; set; }
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public string? ProcessMessage { get; set; }

        public BaseResponse()
        {
            Status = HttpStatusCode.OK;
            IsSuccess = true;
            ProcessMessage = "Process is Successfully";
        }

    }
}

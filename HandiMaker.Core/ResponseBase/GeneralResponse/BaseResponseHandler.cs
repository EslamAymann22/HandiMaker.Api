using System.Net;

namespace HandiMaker.Core.ResponseBase.GeneralResponse
{
    public class BaseResponseHandler
    {

        public BaseResponse<T> Success<T>()
        {
            return new BaseResponse<T>()
            {
                IsSuccess = true,
                Status = HttpStatusCode.OK,
            };
        }

        public BaseResponse<T> Success<T>(T TData)
        {
            return new BaseResponse<T>()
            {
                IsSuccess = true,
                Status = HttpStatusCode.OK,
                Data = TData
            };
        }

        public BaseResponse<T> Failed<T>(HttpStatusCode statusCode, string Message = "")
        {
            return new BaseResponse<T>()
            {
                IsSuccess = false,
                Status = statusCode,
                ProcessMessage = statusCode.ToString() + " " + Message
            };

        }

        public BaseResponse<T> Created<T>(T TData)
        {
            return new BaseResponse<T>()
            {
                IsSuccess = true,
                Status = HttpStatusCode.OK,
                ProcessMessage = "Created Successfully",
                Data = TData
            };
        }

        public BaseResponse<T> Updated<T>(T TData)
        {
            return new BaseResponse<T>()
            {
                IsSuccess = true,
                Status = HttpStatusCode.OK,
                ProcessMessage = "Updated Successfully",
                Data = TData
            };
        }

        public BaseResponse<T> Deleted<T>()
        {
            return new BaseResponse<T>()
            {
                IsSuccess = true,
                Status = HttpStatusCode.OK,
                ProcessMessage = "Deleted Successfully"
            };
        }

        public BaseResponse<T> NotFound<T>(string message = null)
        {
            return new BaseResponse<T>()
            {
                IsSuccess = false,
                Status = HttpStatusCode.NotFound,
                ProcessMessage = message == null ? "Not Found" : message
            };
        }


    }
}

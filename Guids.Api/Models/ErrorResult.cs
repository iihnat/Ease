

using System.Net;

namespace Guids.Api.Models
{
    public class ErrorResult
    {
        public ErrorResult(HttpStatusCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public HttpStatusCode ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
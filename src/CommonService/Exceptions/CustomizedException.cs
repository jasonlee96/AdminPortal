using CommonService.Enums;
using System.Net;

namespace CommonService.Exceptions
{
    public class CustomizedException : Exception
    {
        public ErrorCode Code { get; set; } = ErrorCode.UNKNOWN;

        public string ErrorMessage { get; set; }

        public CustomizedException(ErrorCode code, string message)
        {
            Code = code;
            ErrorMessage = message;
        }
    }
}

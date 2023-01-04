using System.Net;

namespace CommonService.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;

        public string ReasonPhrase { get; set; }

        public HttpResponseException(HttpResponseMessage msg)
        {
            Status = msg.StatusCode;
            ReasonPhrase = msg.ReasonPhrase;
        }
    }
}

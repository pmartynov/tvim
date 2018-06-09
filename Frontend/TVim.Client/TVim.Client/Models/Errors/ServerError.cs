using System.Net;

namespace TVim.Client.Models.Errors
{
    public class ServerError : ErrorBase
    {
        private HttpStatusCode _responseStatusCode;
        private string _content;

        public ServerError(HttpStatusCode responseStatusCode, string content)
        {
            _responseStatusCode = responseStatusCode;
            _content = content;
        }
    }
}
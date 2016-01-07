using System;
using System.Net;

namespace Lucid.Web.Testing.Http
{
    public class UnexpectedStatusCodeException : Exception
    {
        private Response        _response;
        private HttpStatusCode _expectedStatusCode;

        public Response         Response            { get { return _response; } }
        public HttpStatusCode   ExpectedStatusCode  { get { return _expectedStatusCode; } }

        public UnexpectedStatusCodeException(Response response, HttpStatusCode expectedStatusCode)
            : base(FormatMessage(response, expectedStatusCode))
        {
            _response = response;
            _expectedStatusCode = expectedStatusCode;
        }

        private static string FormatMessage(Response response, HttpStatusCode expectedStatusCode)
        {
            return string.Format("Unexpected status code\nExpected {0} {1}\nActual {2} {3}",
                (int)expectedStatusCode,
                expectedStatusCode,
                response.StatusCode,
                response.StatusDescription);
        }
    }
}

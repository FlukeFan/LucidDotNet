using System;
using System.IO;
using System.Net;
using System.Web;
using Lucid.Web.Testing.Hosting;

namespace Lucid.Web.Testing.Http
{
    [Serializable]
    public class SimulatedHttpClient
    {
        public static HttpStatusCode DefaultGetCode = HttpStatusCode.OK;
        public static HttpStatusCode DefaultPostCode = HttpStatusCode.Redirect;

        public static string LastResponseText { get; protected set; }

        private bool _expectedError;

        public ConsoleWriter ConsoleWriter { get; protected set; }

        public SimulatedHttpClient()
        {
            ConsoleWriter = new ConsoleWriter();
        }

        public void ExpectError()
        {
            _expectedError = true;
        }

        public bool HadExpectedError()
        {
            var hadExpectedError = _expectedError;
            _expectedError = false;
            return hadExpectedError;
        }

        public Response Get(string url)
        {
            return Get(DefaultGetCode, url);
        }

        public Response Get(string url, Action<Request> modifier)
        {
            return Get(DefaultGetCode, url, modifier);
        }

        public Response Get(HttpStatusCode expectedStatusCode, string url)
        {
            return Get(expectedStatusCode, url, r => { });
        }

        public Response Get(HttpStatusCode expectedStatusCode, string url, Action<Request> modifier)
        {
            var request = new Request(url, "GET");
            modifier(request);
            return Process(expectedStatusCode, request);
        }

        public Response Post(string url)
        {
            return Post(DefaultPostCode, url);
        }

        public Response Post(string url, Action<Request> modifier)
        {
            return Post(DefaultPostCode, url, modifier);
        }

        public Response Post(HttpStatusCode expectedStatusCode, string url)
        {
            return Post(expectedStatusCode, url, r => { });
        }

        public Response Post(HttpStatusCode expectedStatusCode, string url, Action<Request> modifier)
        {
            var request = new Request(url, "POST").SetFormUrlEncoded();
            modifier(request);
            return Process(expectedStatusCode, request);
        }

        public Response Process(HttpStatusCode expectedStatusCode, Request request)
        {
            using (var output = new StringWriter())
            {
                var workerRequest = new SimulatedWorkerRequest(request, output);
                HttpRuntime.ProcessRequest(workerRequest);

                var responseText = output.ToString();
                LastResponseText = responseText;

                var response = new Response
                {
                    StatusCode          = workerRequest.StatusCode,
                    StatusDescription   = workerRequest.StatusDescription,
                    Text                = responseText,
                };

                if (response.HttpStatusCode != expectedStatusCode)
                    throw new UnexpectedStatusCodeException(response, expectedStatusCode);

                return response;
            }
        }
    }
}

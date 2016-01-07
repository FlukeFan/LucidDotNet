using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Lucid.Web.Testing.Hosting;

namespace Lucid.Web.Testing.Http
{
    [Serializable]
    public class SimulatedHttpClient
    {
        public static string LastResponseText { get; protected set; }

        public ConsoleWriter ConsoleWriter { get; protected set; }

        public SimulatedHttpClient()
        {
            ConsoleWriter = new ConsoleWriter();
        }

        public Response Get(string url)
        {
            return Process(new Request { Verb = "GET", Url = url });
        }

        public Response Post(string url)
        {
            return Process(new Request { Verb = "POST", Url = url });
        }

        public Response Process(Request request)
        {
            request.Url = request.Url.TrimStart('~', '/');

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

                return response;
            }
        }

        private class SimulatedWorkerRequest : SimpleWorkerRequest
        {
            private Request _request;
            private int     _statusCode;
            private string  _statusDescription;

            public int      StatusCode          { get { return _statusCode; } }
            public string   StatusDescription   { get { return _statusDescription; } }

            public SimulatedWorkerRequest(Request request, TextWriter output)
                : base(request.Url, request.Query, output)
            {
                _request = request;
            }

            public override string GetHttpVerbName()
            {
                return _request.Verb;
            }

            public override void SendStatus(int statusCode, string statusDescription)
            {
                base.SendStatus(statusCode, statusDescription);
                _statusCode = statusCode;
                _statusDescription = statusDescription;
            }
        }
    }
}

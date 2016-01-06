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

        public string Get(string url)
        {
            return Process(new Request { Verb = "GET", Url = url });
        }

        public string Post(string url)
        {
            return Process(new Request { Verb = "POST", Url = url });
        }

        public string Process(Request request)
        {
            request.Url = request.Url.TrimStart('~', '/');

            using (var output = new StringWriter())
            {
                var workerRequest = new SimulatedWorkerRequest(request, output);
                HttpRuntime.ProcessRequest(workerRequest);

                var responseText = output.ToString();
                LastResponseText = responseText;
                return responseText;
            }
        }

        private class SimulatedWorkerRequest : SimpleWorkerRequest
        {
            private Request _request;

            public SimulatedWorkerRequest(Request request, TextWriter output)
                : base(request.Url, request.Query, output)
            {
                _request = request;
            }

            public override string GetHttpVerbName()
            {
                return _request.Verb;
            }
        }
    }
}

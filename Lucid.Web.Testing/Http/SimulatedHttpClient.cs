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
        public ConsoleWriter ConsoleWriter { get; protected set; }

        public SimulatedHttpClient()
        {
            ConsoleWriter = new ConsoleWriter();
        }

        public string Get(string url)
        {
            url = url.TrimStart('~', '/');

            using (var output = new StringWriter())
            {
                var workerRequest = new SimulatedWorkerRequest(url, "", output);
                HttpRuntime.ProcessRequest(workerRequest);

                var responseText = output.ToString();
                return responseText;
            }
        }

        private class SimulatedWorkerRequest : SimpleWorkerRequest
        {
            public SimulatedWorkerRequest(string url, string query, TextWriter output)
                : base(url, query, output)
            {
            }
        }
    }
}

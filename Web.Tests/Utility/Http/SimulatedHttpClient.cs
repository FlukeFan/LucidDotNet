using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace Lucid.Web.Tests.Utility.Http
{
    [Serializable]
    public class SimulatedHttpClient
    {
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

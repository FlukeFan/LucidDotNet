using System;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Tests.Testing.Html
{
    public class FakeClient : ISimulatedHttpClient
    {
        public Request Request;

        public Response Process(Request request, Action<Request> modifier)
        {
            Request = request;
            return null;
        }
    }
}

using System;
using Lucid.Web.Testing.Html;
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

        public static Request Do(string html, Action<TypedForm<FormModel>, FakeClient> submit)
        {
            var form = new Response { Text = html }.Form<FormModel>();
            var client = new FakeClient();

            submit(form, client);

            return client.Request;
        }
    }
}

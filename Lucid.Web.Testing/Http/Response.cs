using System;
using System.Net;

namespace Lucid.Web.Testing.Http
{
    [Serializable]
    public class Response
    {
        public int      StatusCode;
        public string   StatusDescription;
        public string   Text;

        public HttpStatusCode HttpStatusCode { get { return (HttpStatusCode)StatusCode; } }
    }
}

using System;
using System.Net;
using AngleSharp.Parser.Html;
using Lucid.Web.Testing.Html;

namespace Lucid.Web.Testing.Http
{
    [Serializable]
    public class Response
    {
        public static Func<HtmlParser> NewParser = () => new HtmlParser();

        [NonSerialized]
        private Lazy<DocumentWrapper> _documentWrapper;

        public Response()
        {
            _documentWrapper = new Lazy<DocumentWrapper>(() =>
            {
                var parser = NewParser();
                var doc = parser.Parse(Text);
                return new DocumentWrapper(doc);
            });

        }

        public int      StatusCode;
        public string   StatusDescription;
        public string   Text;

        public HttpStatusCode   HttpStatusCode  { get { return (HttpStatusCode)StatusCode; } }
        public DocumentWrapper  Doc             { get { return _documentWrapper.Value; } }

        public Form<T> Form<T>()                    { return Doc.Form<T>(); }
        public Form<T> Form<T>(int index)           { return Doc.Form<T>(index); }
        public Form<T> Form<T>(string cssSelector)  { return Doc.Form<T>(cssSelector); }
    }
}

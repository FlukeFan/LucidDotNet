using System;
using System.Net;
using System.Web.Mvc;
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

        public int              StatusCode;
        public string           StatusDescription;
        public string           Text;

        [NonSerialized]
        public ActionResult     ActionResult;

        public HttpStatusCode   HttpStatusCode  { get { return (HttpStatusCode)StatusCode; } }
        public DocumentWrapper  Doc             { get { return _documentWrapper.Value; } }

        public TypedForm<T> Form<T>()                   { return Doc.Form<T>(); }
        public TypedForm<T> Form<T>(int index)          { return Doc.Form<T>(index); }
        public TypedForm<T> Form<T>(string cssSelector) { return Doc.Form<T>(cssSelector); }

        public T ActionResultOf<T>() where T : ActionResult
        {
            return (T)ActionResult;
        }
    }
}

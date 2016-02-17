using AngleSharp.Dom.Html;

namespace Lucid.Web.Testing.Http
{
    public class DocumentWrapper : ParentNodeWrapper
    {
        public DocumentWrapper(IHtmlDocument document) : base(document)
        {
            Document = document;
        }

        public IHtmlDocument Document { get; protected set; }
    }
}

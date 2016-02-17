using AngleSharp.Dom.Html;

namespace Lucid.Web.Testing.Http
{
    public class DocumentWrapper : ParentNodeWrapper
    {
        private IHtmlDocument _document;

        public DocumentWrapper(IHtmlDocument document) : base(document)
        {
            _document = document;
        }

        public IHtmlDocument Document { get { return _document; } }
    }
}

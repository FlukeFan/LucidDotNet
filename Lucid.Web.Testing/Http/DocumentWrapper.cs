using System;
using AngleSharp.Dom.Html;

namespace Lucid.Web.Testing.Http
{
    public class DocumentWrapper
    {
        public DocumentWrapper(IHtmlDocument document)
        {
            Document = document;
        }

        public IHtmlDocument Document { get; protected set; }

        public ElementWrapper FindSingle(string cssSelector)
        {
            var elements = Document.QuerySelectorAll(cssSelector);

            if (elements.Length == 0)
                throw new Exception(string.Format("Could not find any element matching selector '{0}'", cssSelector));

            if (elements.Length > 1)
                throw new Exception(string.Format("Expected 1 element, but found {0} elements matching selector '{1}'", elements.Length, cssSelector));

            return new ElementWrapper(elements[0]);
        }
    }
}

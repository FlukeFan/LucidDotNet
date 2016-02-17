using System;
using AngleSharp.Dom;

namespace Lucid.Web.Testing.Http
{
    public class ElementWrapper
    {
        public ElementWrapper(IElement element)
        {
            Element = element;
        }

        public IElement Element { get; protected set; }

        public string TextContent { get { return Element.TextContent; } }

        public string FindAttribute(string attributeName)
        {
            var attribute = Element.Attributes[attributeName];

            if (attribute == null)
                throw new Exception(string.Format("Could not find attribute '{0}' on element {1}", attributeName, FormatTag()));

            return attribute.Value;
        }

        public ElementWrapper And(Action<ElementWrapper> verification)
        {
            verification(this);
            return this;
        }

        private string FormatTag()
        {
            var html = Element.OuterHtml;
            var firstTagClose = html.IndexOf('>');
            return html.Substring(0, firstTagClose + 1);
        }
    }
}

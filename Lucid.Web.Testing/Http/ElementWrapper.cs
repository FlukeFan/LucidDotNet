using System;
using AngleSharp.Dom;

namespace Lucid.Web.Testing.Http
{
    public class ElementWrapper : ParentNodeWrapper
    {
        public ElementWrapper(IElement element) : base(element)
        {
            Element = element;
        }

        public IElement Element { get; protected set; }

        public string TextContent { get { return Element.TextContent; } }

        public string Attribute(string attributeName)
        {
            var attribute = Element.Attributes[attributeName];

            if (attribute == null)
                throw new Exception(string.Format("Could not find attribute '{0}' on element {1}", attributeName, FormatTag()));

            return attribute.Value;
        }

        public ElementWrapper Where(Action<ElementWrapper> verification)
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

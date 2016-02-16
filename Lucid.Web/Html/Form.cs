using System;
using System.Web.Mvc;
using HtmlTags;

namespace Lucid.Web.Html
{
    public class Form : IDisposable
    {
        private HtmlHelper  _helper;
        private FormTag     _formTag;

        public Form(HtmlHelper helper)
        {
            _helper = helper;

            _formTag = new FormTag();
            _helper.ViewContext.Writer.Write(_formTag.NoClosingTag().ToString());
        }

        void IDisposable.Dispose()
        {
            _helper.ViewContext.Writer.Write("</form>");
        }
    }
}

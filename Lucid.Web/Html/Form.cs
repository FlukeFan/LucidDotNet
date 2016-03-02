using System;
using System.Web.Mvc;
using HtmlTags;

namespace Lucid.Web.Html
{
    public class Form<T> : IDisposable
    {
        private HtmlHelper<T>   _helper;
        private FormTag         _formTag;

        public Form(HtmlHelper<T> helper, Action<FormTag> mutator = null)
        {
            _helper = helper;

            var url = helper.ViewContext.RequestContext.HttpContext.Request.RawUrl;

            _formTag = new FormTag();
            _formTag.Action(url);

            if (mutator != null)
                mutator(_formTag);

            RenderFormStart();
        }

        protected virtual void RenderFormStart()
        {
            _helper.ViewContext.Writer.Write(_formTag.NoClosingTag().ToString());
        }

        public HtmlHelper<T> Html { get { return _helper; } }

        void IDisposable.Dispose()
        {
            _helper.ViewContext.Writer.Write("</form>");
        }
    }
}

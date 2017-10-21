using System.Web.Mvc;
using MvcForms;

namespace Lucid.Web.App.Helpers
{
    public static class LucidHelpers
    {
        public static ScopedHtmlHelper<T> BeginFormButtons<T>(this HtmlHelper<T> helper)
        {
            helper.ViewContext.Writer.Write("<div class=\"row\"><div class=\"col-sm-offset-4 col-sm-8\">");
            return new ScopedHtmlHelper<T>(helper, () =>
            {
                helper.ViewContext.Writer.Write("</div></div>");
            });
        }
    }
}
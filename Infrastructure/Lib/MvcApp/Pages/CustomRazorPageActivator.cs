using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Lucid.Infrastructure.Lib.MvcApp.Pages
{
    public class CustomRazorPageActivator : RazorPageActivator, IRazorPageActivator
    {
        public const string IsMvcAppPage = "IsMvcAppPage";

        public CustomRazorPageActivator(IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, IJsonHelper jsonHelper, DiagnosticSource diagnosticSource, HtmlEncoder htmlEncoder, IModelExpressionProvider modelExpressionProvider)
            : base(metadataProvider, urlHelperFactory, jsonHelper, diagnosticSource, htmlEncoder, modelExpressionProvider)
        { }

        void IRazorPageActivator.Activate(IRazorPage page, ViewContext context)
        {
            Activate(page, context);

            //if (page is IMvcAppPage)
            //    context.HttpContext.Items.Add(IsMvcAppPage, true);
        }
    }
}

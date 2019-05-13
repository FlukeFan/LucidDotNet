using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Host.Web.Layout
{
    public static class RazorExtensions
    {
        // from:  https://stackoverflow.com/a/45088852/357728
        public static string AddFileVersionToPath(this HttpContext context, string path)
        {
            var versionProvider = context.RequestServices.GetRequiredService<IFileVersionProvider>();
            return versionProvider.AddFileVersionToPath(context.Request.Path, path);
        }
    }
}

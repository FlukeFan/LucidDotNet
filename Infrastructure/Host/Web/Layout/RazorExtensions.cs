using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Infrastructure.Host.Web.Layout
{
    public static class RazorExtensions
    {
        // from:  https://stackoverflow.com/a/45088852/357728
        public static string AddFileVersionToPath(this HttpContext context, string path)
        {
            IMemoryCache cache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var hostingEnvironment = context.RequestServices.GetRequiredService<IHostingEnvironment>();
            var versionProvider = new FileVersionProvider(hostingEnvironment.WebRootFileProvider, cache, context.Request.Path);
            return versionProvider.AddFileVersionToPath(path);
        }
    }
}

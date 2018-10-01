using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public static class RazorFolderExtensions
    {
        public static ApplicationPartManager UseCompiledRazorViews(this ApplicationPartManager apm)
        {
            var razorParts = apm.ApplicationParts
                .OfType<IRazorCompiledItemProvider>()
                .ToList();

            foreach (var razorPart in razorParts)
            {
                apm.ApplicationParts.Remove((ApplicationPart)razorPart);
                var customPart = new WrappedRazorCompiledItemProvider(razorPart);
                apm.ApplicationParts.Add(customPart);
            }

            return apm;
        }

        public static IServiceCollection UseFileSystemRazorViews(this IServiceCollection services, string rootSourcePath)
        {
            services.Configure<RazorViewEngineOptions>(o => o.FileProviders.Add(new ModuleRazorFileProvider(rootSourcePath)));
            return services;
        }
    }
}

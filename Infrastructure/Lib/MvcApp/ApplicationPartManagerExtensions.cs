using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public static class ApplicationPartManagerExtensions
    {
        public static ApplicationPartManager AddModuleFeatureFolders(this ApplicationPartManager apm)
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
    }
}

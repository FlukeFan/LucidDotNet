using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Lib.MvcApp.RazorFolders
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

        public static IServiceCollection UseCustomFileSystemRazorViews(this IServiceCollection services, string rootSourcePath)
        {
            services.Configure<RazorViewEngineOptions>(o => o.FileProviders.Add(new ModuleRazorFileProvider(rootSourcePath)));
            return services;
        }

        public static string RelativeViewPath<T>(this T typeInNamespace, string viewName)
        {
            return typeof(T).RelativeViewPath(viewName);
        }

        public static string AbsoluteViewPath<T>(this T typeFromAssembly, string viewPath)
        {
            return typeof(T).AbsoluteViewPath(viewPath);
        }

        public static string RelativeViewPath(this Type typeInNamespace, string viewName)
        {
            var assembly = typeInNamespace.Assembly.GetName().Name;
            var controllerPath = typeInNamespace.Namespace.Substring(assembly.Length).Replace(".", "/");
            return $"/{assembly}{controllerPath}/{viewName.TrimStart('/')}";
        }

        public static string AbsoluteViewPath(this Type typeFromAssembly, string viewPath)
        {
            var assembly = typeFromAssembly.Assembly.GetName().Name;
            return $"/{assembly}/{viewPath.TrimStart('/')}";
        }
    }
}

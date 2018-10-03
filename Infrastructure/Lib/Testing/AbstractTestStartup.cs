using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Infrastructure.Lib.Testing
{
    public abstract class AbstractTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRazorPageActivator, CustomRazorPageActivator>()
                .AddMvc(o =>
                {
                    o.Filters.Add(new FeatureFolderViewFilter());
                    o.Filters.Add(new MvcAppPageResultFilter(false));
                })
                .ConfigureApplicationPartManager(apm => apm.UseCompiledRazorViews());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}

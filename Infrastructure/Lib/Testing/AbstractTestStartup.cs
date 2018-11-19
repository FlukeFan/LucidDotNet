using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using MvcTesting.AspNetCore;

namespace Lucid.Infrastructure.Lib.Testing
{
    public abstract class AbstractTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRazorPageActivator, CustomRazorPageActivator>()
                .AddSingleton<ISetLayout, EmptySetLayout>()
                .AddMvc(o =>
                {
                    o.Filters.Add(new FeatureFolderViewFilter());
                    o.Filters.Add(new MvcAppPageResultFilter(false));
                    o.Filters.Add(new CaptureResultFilter());
                })
                .ConfigureApplicationPartManager(apm => apm.UseCompiledRazorViews());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}

using Lucid.Lib.MvcApp.Pages;
using Lucid.Lib.MvcApp.RazorFolders;
using Lucid.Lib.Testing.Controller;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MvcTesting.AspNetCore;

namespace Lucid.Lib.Testing
{
    public abstract class AbstractTestStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISetLayout>(NewSetLayout());
            services.AddRazorPages();
            services.AddControllersWithViews(o =>
            {
                o.Filters.Add(new StubUserFilter());
                o.Filters.Add(new FeatureFolderViewFilter());
                o.Filters.Add(new CaptureResultFilter());
            })
                .ConfigureApplicationPartManager(apm => apm.UseCompiledRazorViews());
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
                cfg.MapRazorPages();
            });
        }

        protected virtual ISetLayout NewSetLayout() { return new EmptySetLayout(); }
    }
}

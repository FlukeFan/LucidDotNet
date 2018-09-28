using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Infrastructure.Lib.Testing
{
    public abstract class AbstractTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(o => o.Filters.Add(new FeatureFolderViewFilter()))
                .ConfigureApplicationPartManager(apm => apm.AddModuleFeatureFolders());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
            });
        }
    }
}

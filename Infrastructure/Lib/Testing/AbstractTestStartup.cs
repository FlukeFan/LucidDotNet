using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Infrastructure.Lib.Testing
{
    public abstract class AbstractTestStartup
    {
        private string _namespacePrefix;

        public AbstractTestStartup(string namespacePrefix)
        {
            _namespacePrefix = namespacePrefix;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o => o.Filters.Add(new FeatureFolderViewFilter(_namespacePrefix, "", "/")));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
            });
        }
    }
}

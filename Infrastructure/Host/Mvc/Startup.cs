using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipDeploy;

namespace Lucid.Infrastructure.Host.Mvc
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseZipDeploy(opt => opt.UseIisUrl("http://lucid.rgbco.uk"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
            });
        }
    }

    [Route("/err")]
    public class TempError : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new System.Exception("testing errors");
        }
    }
}

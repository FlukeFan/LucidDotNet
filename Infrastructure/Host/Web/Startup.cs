using System;
using System.IO;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipDeploy;

namespace Lucid.Infrastructure.Host.Web
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc(o => ConfigureMvcOptions(o));

            if (_env.IsDevelopment())
            {
                // use the file-system razor views so that we get re-compilation when they change
                var rootSourcePath = Path.GetFullPath(Path.Combine(_env.ContentRootPath, "../../.."));
                services.UseFileSystemRazorViews(rootSourcePath);
            }
            else
            {
                // use the compiled razor views
                mvc.ConfigureApplicationPartManager(apm => apm.UseCompiledRazorViews());
            }
        }

        protected virtual void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(new FeatureFolderViewFilter());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseZipDeploy(opt => opt
                .UseIisUrl("https://lucid.rgbco.uk")
                .UseIsBinary(f => ZipDeployOptions.DefaultIsBinary(f) || Path.GetFileName(f) == "nlog.config"));

            if (_env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }

    [Route("/err")]
    public class TempError : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new Exception("testing errors");
        }
    }
}

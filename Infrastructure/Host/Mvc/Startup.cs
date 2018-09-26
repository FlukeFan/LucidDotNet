﻿using System;
using System.IO;
using Lucid.Infrastructure.Lib.MvcApp;
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
            services.AddMvc(o => o.Filters.Add(new FeatureFolderViewFilter("Lucid.Modules", "Module/", "/ModuleViews")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseZipDeploy(opt => opt
                .UseIisUrl("https://lucid.rgbco.uk")
                .UseIsBinary(f => ZipDeployOptions.DefaultIsBinary(f) || Path.GetFileName(f) == "nlog.config"));

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
            throw new Exception("testing errors");
        }
    }
}

﻿using System;
using System.IO;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Lucid.Infrastructure.Host.Web.Layout;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvcForms;
using MvcForms.Styles.Bootstrap;
using ZipDeploy;

namespace Lucid.Infrastructure.Host.Web
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private ILogger             _log;

        public Startup(IHostingEnvironment env, ILogger<Startup> log)
        {
            _env = env;
            _log = log;

            _log.LogInformation($"Startup environment: {_env.EnvironmentName}");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISetLayout, SetLayout>();

            var mvc = services.AddMvc(o => ConfigureMvcOptions(o));

            if (_env.IsDevelopment())
            {
                // use the file-system razor views so that we get re-compilation when they change
                var rootSourcePath = Path.GetFullPath(Path.Combine(_env.ContentRootPath, "../../.."));
                services.UseCustomFileSystemRazorViews(rootSourcePath);
            }
            else
            {
                mvc.ConfigureApplicationPartManager(apm => apm.UseCompiledRazorViews());
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.Cookie.Name = "AuthCookie";
                    o.LoginPath = Modules.Security.Actions.Login();
                });
        }

        protected virtual void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(new FeatureFolderViewFilter());
            mvcOptions.Filters.Add(new AuthorizeFilter());
            mvcOptions.Filters.Add(new PjaxFilter());
        }

        public void Configure(IApplicationBuilder app, IConfiguration config, ILoggerFactory loggerFactory)
        {
            app.UseZipDeploy(opt => opt
                .UseIisUrl("https://lucid.rgbco.uk")
                .UseIsBinary(f => ZipDeployOptions.DefaultIsBinary(f) || Path.GetFileName(f) == "nlog.config"));

            var hostConfig = config.GetSection("Host");
            var securitySchema = new Schema { Name = "Security" };
            var designSchema = new Schema { Name = "Design" };

            InitSqlServer(hostConfig.GetSection("SqlServer"),
                securitySchema,
                designSchema);

            app.UseAuthentication();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();
            app.UseMvc();
            Styler.Set(new Bootstrap4Style());

            var startupTasks = new[]
            {
                Task.Run(() => Modules.ProjectCreation.Registry.Startup()),
                Task.Run(() => Modules.Security.Registry.Startup(securitySchema, loggerFactory.CreateLogger<MigrationRunner>())),
                Task.Run(() => Modules.AppFactory.Design.Registry.Startup(designSchema, loggerFactory.CreateLogger<MigrationRunner>())),
            };

            Task.WaitAll(startupTasks);

            var startupTime = Program.StartupCompleted();
            loggerFactory.CreateLogger("SystemAlert").LogInformation($"Startup complete: {startupTime}");
        }

        protected virtual void InitSqlServer(IConfigurationSection config, params Schema[] schemas)
        {
            var server = config.GetValue<string>("Server");
            var dbName = config.GetValue<string>("DbName");
            var userId = config.GetValue<string>("UserId");
            var password = config.GetValue<string>("Password");

            var startup = new SqlServer(server, dbName, userId, password);

            var createDb = config.GetValue<bool>("CreateDb");
            startup.SetSchemaConnections(schemas);
            startup.CreateSchemas(createDb, schemas);
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

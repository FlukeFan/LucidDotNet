using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Lucid.Infrastructure.Host.Web.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Lucid.Infrastructure.Host.Web
{
    public class Program
    {
        private static Stopwatch _startupTimer;

        public static TimeSpan? StartupCompleted()
        {
            _startupTimer?.Stop();
            return _startupTimer?.Elapsed;
        }

        public static void Main(string[] args)
        {
            _startupTimer = new Stopwatch();
            _startupTimer.Start();

            IList<string> logSetupMessages = new List<string>();
            var logConfigFile = LogConfig.PrepareConfigFile(logSetupMessages);

            if (logConfigFile != null)
                NLogBuilder.ConfigureNLog(logConfigFile);

            var logger = LogManager.GetCurrentClassLogger();

            foreach (var logSetupMessage in logSetupMessages)
                logger.Debug($"log setup: {logSetupMessage}");

            logger.Info($"Logging configured using '{logConfigFile}'");

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Fatal(e, $"error from web host");
                throw;
            }
            finally
            {
                logger.Info($"Shutting down logging");
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSetting(WebHostDefaults.CaptureStartupErrorsKey, "false")
                .ConfigureAppConfiguration(AddConfig)
                .ConfigureLogging(l => l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace))
                .UseNLog()
                .UseStartup<Startup>();

        private static void AddConfig(IConfigurationBuilder config)
        {
            var cd = Directory.GetCurrentDirectory();
            var configFile = Path.Combine(cd, "web.config.xml");

            while (!File.Exists(configFile))
            {
                var parent = Directory.GetParent(cd)?.FullName;

                if (parent == cd || parent == null)
                    throw new Exception($"web.config.xml not found in parent of {Directory.GetCurrentDirectory()}");

                cd = parent;
                configFile = Path.Combine(cd, "web.config.xml");
            }

            config.AddXmlFile(configFile);
            config.AddEnvironmentVariables();
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Web;

namespace Lucid.Infrastructure.Host.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IList<string> logSetupMessages = new List<string>();
            var logConfigFile = Logging.PrepareConfigFile(logSetupMessages);

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
                .UseNLog()
                .UseStartup<Startup>();
    }
}

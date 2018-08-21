using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Web;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (File.Exists("nlog.config"))
                NLogBuilder.ConfigureNLog("nlog.config");

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info($"Creating web host");

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "caught error");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseNLog()
                .UseStartup<Startup>();
    }
}

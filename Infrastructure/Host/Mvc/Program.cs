﻿using System;
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
            Logging.Configure();

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info($"Creating web host");

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

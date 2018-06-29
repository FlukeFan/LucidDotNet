﻿using System;
using System.Diagnostics;
using MvcTesting.Hosting;
using MvcTesting.Http;
using NUnit.Framework;
using SimpleFacade.Testing;

namespace Lucid.Web.Tests.Utility
{
    [TestFixture]
    public abstract class WebTest
    {
        private static AspNetTestHost _webApp;

        protected ExecutorStub ExecutorStub {[DebuggerStepThrough] get { return WebTestRegistry.ExecutorStub; } }

        public static void SetUpWebHost()
        {
            _webApp = AspNetTestHost.For(@"..\..\..\Lucid.Web", typeof(TestEnvironmentProxy));
        }

        public static void TearDownWebHost()
        {
            using (_webApp) { }
        }

        protected void WebAppTest(Action<SimulatedHttpClient> test)
        {
            _webApp.Test(http =>
            {
                WebTestRegistry.SetupExecutorStub();
                test(http);
            });
        }

        private class TestEnvironmentProxy : AppDomainProxy
        {
            public TestEnvironmentProxy()
            {
                LucidApplication.Startup = new FakeStartup();
            }
        }
    }
}
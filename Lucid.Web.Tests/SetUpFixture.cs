﻿using Lucid.Web.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [SetUp]
        public void SetUp()
        {
            try
            {
                WebTest.SetUpWebHost();
            }
            catch
            {
                WebTest.TearDownWebHost();
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            WebTest.TearDownWebHost();
        }
    }
}
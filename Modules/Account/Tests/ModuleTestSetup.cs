﻿using System;
using System.IO;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using MvcTesting.AspNetCore;
using NHibernate;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.Account.Tests
{
    [SetUpFixture]
    public class ModuleTestSetup
    {
        public static ConstraintChecker MemoryConstraints = new ConstraintChecker();

        private static Lazy<ISessionFactory>            _sessionFactory;
        private static SetupTestServer<TestStartup>     _testServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var schema =
                SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateUserTable>(
                    schemaName:             "Account",
                    migrationsSourceFolder: Path.Combine(TestUtil.ProjectPath(), "../Module/DbMigrations"));

            _testServerSetup = new SetupTestServer<TestStartup>();

            _sessionFactory = new Lazy<ISessionFactory>(() => Registry.BuildSessionFactory(schema.ConnectionString));
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            using (_testServerSetup) { }

            if (_sessionFactory.IsValueCreated)
                using (_sessionFactory.Value) { }
        }

        [TestFixture]
        public abstract class ControllerTest
        {
            private SetupExecutorStub _excecutorStub;

            protected SimulatedHttpClient   MvcTestingClient()  { return _testServerSetup.TestServer.MvcTestingClient(); }
            protected ExecutorStub          ExecutorStub        => _excecutorStub.Stub;

            [SetUp]
            public void SetUp()
            {
                _excecutorStub = new SetupExecutorStub(Registry.Executor, e => Registry.Executor = e);
            }

            [TearDown]
            public void TearDown()
            {
                using (_excecutorStub) { }
            }
        }

        public class MemoryRepo : IDisposable
        {
            public MemoryRepo()
            {
                MemoryRepository = new MemoryRepository(MemoryConstraints);
            }

            public MemoryRepository MemoryRepository { get; private set; }

            public void Dispose() { }
        }

        public class DbTxTest : NhSqlTxTest
        {
            public DbTxTest() : base(_sessionFactory.Value) { }
        }

        public class TestStartup : AbstractTestStartup
        {
        }
    }
}

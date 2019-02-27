using System;
using System.IO;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Domain;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using MvcTesting.AspNetCore;
using NHibernate;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.Security.Tests
{
    [SetUpFixture]
    public class ModuleTestSetup
    {
        public static ConstraintChecker MemoryConstraints = new ConstraintChecker();

        private static DbState                          _dbState             = new DbState();
        private static Lazy<ISessionFactory>            _sessionFactory;
        private static SetupTestServer<TestStartup>     _testServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var schema =
                SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateUserTable>(
                    schemaName:             "Security",
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
            protected ExecutorStubAsync     ExecutorStub        => _excecutorStub.Stub;

            [SetUp]
            public void SetUp()
            {
                _excecutorStub = new SetupExecutorStub(Registry.ExecutorAsync, e => Registry.ExecutorAsync = e);
            }

            [TearDown]
            public void TearDown()
            {
                using (_excecutorStub) { }
            }
        }

        [TestFixture]
        public abstract class LogicTest
        {
            private SetupMemoryLogic _memoryLogic;

            [SetUp]
            public void SetUp()
            {
                _memoryLogic = new SetupMemoryLogic();
            }

            [TearDown]
            public void TearDown()
            {
                using (_memoryLogic) { }
            }
        }

        public class SetupMemoryLogic : SetupDomainRegistry
        {
            private INhSqlRepository    _previousRepository;

            public SetupMemoryLogic()
            {
                _previousRepository = Registry.Repository.Value;
                MemoryRepository = new NhSqlMemoryRepository(MemoryConstraints);
                Registry.Repository.Value = MemoryRepository;
            }

            public NhSqlMemoryRepository MemoryRepository { get; private set; }

            public override void Dispose()
            {
                Registry.Repository.Value = _previousRepository;
                base.Dispose();
            }
        }

        public class SetupDbTx : SetupNhSqlTx
        {
            public SetupDbTx() : base(_sessionFactory.Value, _dbState) { }
        }

        public class TestStartup : AbstractTestStartup
        {
            public const string AuthCookieName = "TestStartupAuthCookie";

            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(o => { o.Cookie.Name = AuthCookieName; });

                base.ConfigureServices(services);
            }
        }
    }
}

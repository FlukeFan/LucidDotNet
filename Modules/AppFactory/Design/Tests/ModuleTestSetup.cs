using System;
using System.IO;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using MvcTesting.AspNetCore;
using NHibernate;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    [SetUpFixture]
    public class ModuleTestSetup
    {
        public static ConstraintChecker MemoryConstraints = new ConstraintChecker();

        private static DbState                          _dbState            = new DbState();
        private static SetupTestServer<TestStartup>     _testServerSetup;
        private static Lazy<ISessionFactory>            _sessionFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var schema =
                SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateBlueprintTable>(
                    schemaName:             "Design",
                    migrationsSourceFolder: Path.Combine(TestUtil.ProjectPath(), "../Module/DbMigrations"));

            _sessionFactory = new Lazy<ISessionFactory>(() => Registry.BuildSessionFactory(schema.ConnectionString));
            _testServerSetup = new SetupTestServer<TestStartup>();
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
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
                StubUserFilter.SetupDefault();
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

        public class SetupMemoryLogic : IDisposable
        {
            private Func<DateTime>      _previousNow;
            private INhSqlRepository    _previousRepository;

            public SetupMemoryLogic()
            {
                _previousNow = Registry.UtcNow;
                _previousRepository = Registry.Repository.Value;
                MemoryRepository = new NhSqlMemoryRepository(MemoryConstraints);
                Registry.Repository.Value = MemoryRepository;
            }

            public NhSqlMemoryRepository MemoryRepository { get; private set; }

            public void Dispose()
            {
                Registry.Repository.Value = _previousRepository;
                Registry.UtcNow = _previousNow;
            }
        }

        public class SetupDbTx : SetupNhSqlTx
        {
            private INhSqlRepository _previousRepository;

            public SetupDbTx() : base(_sessionFactory.Value, _dbState)
            {
                _previousRepository = Registry.Repository.Value;
                Registry.Repository.Value = NhRepository;
            }

            public override void Clean(NhSqlRepository repository)
            {
                repository.Session.Delete($"from {nameof(Design.Blueprints.Blueprint)}");
            }

            public override void Dispose()
            {
                Registry.Repository.Value = _previousRepository;
                base.Dispose();
            }
        }

        public class TestStartup : AbstractTestStartup { }
    }
}

using System;
using System.IO;
using Lucid.Lib.Domain.SqlServer;
using Lucid.Lib.Facade;
using Lucid.Lib.Testing;
using Lucid.Lib.Testing.Controller;
using Lucid.Lib.Testing.Execution;
using Lucid.Lib.Testing.SqlServer;
using Lucid.Lib.Util;
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
            ProcessLocks.AddDb("Design");

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
            private IExecutorAsync      _previousExecutorAsync;

            public SetupMemoryLogic()
            {
                _previousNow = Registry.UtcNow;
                _previousRepository = Registry.Repository.Value = MemoryRepository = new NhSqlMemoryRepository(MemoryConstraints);
                _previousExecutorAsync = Registry.ExecutorAsync = new ExecutorAsync().UsingHandlersFromAssemblyWithType<Registry>();
            }

            public NhSqlMemoryRepository MemoryRepository { get; private set; }

            public void Dispose()
            {
                Registry.ExecutorAsync = _previousExecutorAsync;
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

using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Execution;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ModuleTest
    {
        public abstract class Controller : ModuleControllerTests<TestStartup>
        {
            protected ExecutorStub ExecutorStub { get; private set; }

            [SetUp]
            public void SetUp()
            {
                Registry.Executor = ExecutorStub = new ExecutorStub();
            }
        }

        public class TestStartup : AbstractTestStartup
        {
        }
    }
}

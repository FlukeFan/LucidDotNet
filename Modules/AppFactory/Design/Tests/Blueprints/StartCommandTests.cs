using System;
using Lucid.Infrastructure.Lib.Testing.Validation;
using Lucid.Modules.AppFactory.Design.Tests;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Blueprints.Tests
{
    [TestFixture]
    public class StartCommandTests : ModuleTestSetup.LogicTest
    {
        private static readonly Func<StartCommand> _validCommand = () => new StartCommand { Name = "TestBlueprint1" };

        [Test]
        public void Validation()
        {
            _validCommand().ShouldBeInvalid(c => c.Name = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.Name = "", "Name cannot be empty");
        }
    }
}

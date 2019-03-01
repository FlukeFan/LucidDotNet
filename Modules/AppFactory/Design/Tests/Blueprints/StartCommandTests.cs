using System;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Testing.Validation;
using Lucid.Modules.AppFactory.Design.Blueprints;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    [TestFixture]
    public class StartCommandTests : ModuleTestSetup.LogicTest
    {
        private static readonly Func<StartCommand> _validCommand = Agreements.Start.Executable;

        [Test]
        public void Validation()
        {
            _validCommand().ShouldBeValid();
            _validCommand().ShouldBeInvalid(c => c.Name = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.Name = "", "Name cannot be empty");
        }

        [Test]
        public void WhenUserIdNotSpecified_Throws()
        {
            var cmd = _validCommand();
            cmd.OwnedByUserId = 0;

            Assert.ThrowsAsync<FacadeException>(async () =>
            {
                await cmd.ExecuteAsync();
            });
        }
    }
}

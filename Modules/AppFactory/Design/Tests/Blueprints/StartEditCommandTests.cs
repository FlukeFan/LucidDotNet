using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Testing.Validation;
using Lucid.Modules.AppFactory.Design.Blueprints;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    [TestFixture]
    public class StartEditCommandTests : ModuleTestSetup.LogicTest
    {
        private static readonly Func<StartEditCommand> _validCommand = Agreements.Start.Executable;

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

            Assert.That(() =>
                cmd.ExecuteAsync(),
                Throws.InstanceOf<FacadeException>());
        }

        [Test]
        public async Task Agreement()
        {
            var agreement = Agreements.Start;

            var blueprint = await agreement.Executable().ExecuteAsync();

            blueprint.Should().BeEquivalentTo(agreement.Result(), opt => opt.Excluding(o => o.Id));
        }

        [Test]
        public async Task Start_DuplicateName_Throws()
        {
            await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_duplicate" }.ExecuteAsync();

            Assert.That(() =>
                new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_duplicate" }.ExecuteAsync(),
                Throws.InstanceOf<FacadeException>());
        }

        [Test]
        public async Task Start_DuplicateNameWithDifferentUser_StartsBlueprint()
        {
            await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_unique" }.ExecuteAsync();
            await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_duplicate" }.ExecuteAsync();
            await new StartEditCommand { OwnedByUserId = 234, Name = "Blueprint_duplicate" }.ExecuteAsync();
        }
    }
}

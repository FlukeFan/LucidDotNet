
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Facade.Pledge;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.Validation;
using Lucid.Infrastructure.Lib.Util;
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
            _validCommand().ShouldBeInvalid(c => c.Name = Defaults.LongString, "Name cannot be too long");
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
        public async Task Start_Agreement()
        {
            var agreement = Agreements.Start;

            var blueprintId = await agreement.Executable().ExecuteAsync();

            blueprintId.Should().NotBe(0);

            await new FindBlueprintQuery { BlueprintId = blueprintId }.ExecuteAsync()
                .Then(dto => dto.Should().BeEquivalentTo(new BlueprintDtoDefault { Id = blueprintId }));
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

        [Test]
        public async Task Edit_Agreement()
        {
            var agreement = Agreements.Edit;

            var existingBlueprintId = await Agreements.Start.Executable().ExecuteAsync();

            var updatedBlueprintId = await agreement.Executable().Mutate(c => c.BlueprintId = existingBlueprintId).ExecuteAsync();

            updatedBlueprintId.Should().Be(existingBlueprintId);

            await new FindBlueprintQuery { BlueprintId = updatedBlueprintId }.ExecuteAsync()
                .Then(dto => dto.Should().BeEquivalentTo(new BlueprintDtoDefault
                {
                    Id = updatedBlueprintId,
                    Name = "UpdatedBlueprint",
                }));
        }

        [Test]
        public async Task Edit_NoChanges()
        {
            var bpId = await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_1" }.ExecuteAsync();

            await new StartEditCommand
            {
                BlueprintId = bpId,
                OwnedByUserId = 123,
                Name = "Blueprint_1",
            }.ExecuteAsync();
        }

        [Test]
        public async Task Edit_DuplicateName_Throws()
        {
            await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_1" }.ExecuteAsync();
            var bp2Id = await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint_2" }.ExecuteAsync();

            Assert.That(() =>
                new StartEditCommand { BlueprintId = bp2Id, OwnedByUserId = 123, Name = "Blueprint_1" }.ExecuteAsync(),
                Throws.InstanceOf<FacadeException>());
        }

        [Test]
        public async Task Edit_NotOwner_Throws()
        {
            var bpId = await new StartEditCommand { OwnedByUserId = 123, Name = "Blueprint" }.ExecuteAsync();

            Assert.That(() =>
                new StartEditCommand { BlueprintId = bpId, OwnedByUserId = 234, Name = "Blueprint" }.ExecuteAsync(),
                Throws.InstanceOf<Exception>());
        }
    }
}

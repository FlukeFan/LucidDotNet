using FluentAssertions;
using Lucid.Domain.Account;
using Lucid.Domain.Contract;
using Lucid.Domain.Tests.Account;
using Lucid.Infrastructure.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Infrastructure.Tests.Account
{
    [TestFixture]
    public class AccountMappingTests : DbTest
    {
        [Test]
        public void User_SaveLoad()
        {
            var user = new UserBuilder().Value();

            Repository.Save(user);

            Repository.Flush();
            Repository.Clear();

            var loadedUser = Repository.Load<User>(user.Id);

            loadedUser.Id.Should().Be(user.Id);
            loadedUser.Name.Should().Be(user.Name);
            loadedUser.LastLoggedIn.Should().Be(user.LastLoggedIn);
        }

        [Test]
        public void User_VerifyConstraints()
        {
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Name, null).Value());
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Name, new string('x', Constraints.DefaultMaxStringLength + 1)).Value());
        }
    }
}

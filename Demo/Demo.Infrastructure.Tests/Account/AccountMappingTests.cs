using Demo.Domain.Account;
using Demo.Domain.Tests.Account;
using Demo.Infrastructure.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Infrastructure.Tests.Account
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
            loadedUser.Email.Should().Be(user.Email);
            loadedUser.LastLoggedIn.Should().Be(user.LastLoggedIn);
        }

        [Test]
        public void User_VerifyConstraints()
        {
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Email, null).Value());
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Email, new string('x', 256)).Value());
        }
    }
}

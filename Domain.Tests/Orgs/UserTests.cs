using System;
using FluentAssertions;
using Lucid.Domain.Orgs;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Orgs
{
    public class UserBuilder : Builder<User>
    {
    }

    [TestFixture]
    public class UserTests : DomainTest
    {
        [Test]
        public void Login_WhenDoesNotExist_IsCreated()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var user = User.Login("does.not@exist.net");

            Repository.ShouldContain(user);

            user.LastLoggedIn.Should().Be(now);
        }

        [Test]
        public void Login_WhenAlreadyExists_SavedInstanceIsReturned()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var existingUser =
                new UserBuilder()
                    .With(u => u.Email, "existing@user.net")
                    .With(u => u.LastLoggedIn, Test.SummerDateTime1 - TimeSpan.FromHours(1))
                    .Save();

            var user = User.Login("existing@user.net");

            user.Should().BeSameAs(existingUser);
            user.LastLoggedIn.Should().Be(now);
        }
    }
}

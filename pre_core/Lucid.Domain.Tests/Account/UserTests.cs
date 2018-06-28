using System;
using FluentAssertions;
using Lucid.Domain.Account;
using Lucid.Domain.Contract;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Account
{
    public class UserBuilder : Builder<User>
    {
        static UserBuilder()
        {
            LucidCustomChecks.Add<User>((validator, user) =>
            {
                validator.CheckNotNull(() => user.Name);
                validator.CheckMaxLength(() => user.Name, Constraints.DefaultMaxStringLength);
            });
        }

        public UserBuilder()
        {
            With(u => u.Name, "TestName");
            With(u => u.LastLoggedIn, TestValues.EarlyDateTimeValue);
        }
    }

    public class UserTests : DomainTest
    {
        [Test]
        public void LoginCreatesNewUser_WhenUserDoesNotExist()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var user = User.Login("DoesNotExist");

            Repository.ShouldContain(user);

            user.LastLoggedIn.Should().Be(now);
        }

        [Test]
        public void LoginReturnsExistingUser_WhenUserAlreadyExists()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var existingUser =
                new UserBuilder()
                    .With(u => u.Name, "ExistingUser")
                    .With(u => u.LastLoggedIn, Test.SummerDateTime1 - TimeSpan.FromHours(1))
                    .Save();

            var anotherUser = new UserBuilder().With(u => u.Name, "AnotherUser").Save();

            var user = User.Login("ExistingUser");

            user.Should().BeSameAs(existingUser);
            user.LastLoggedIn.Should().Be(now);
        }
    }
}

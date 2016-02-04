using System;
using Demo.Domain.Account;
using Demo.Domain.Contract;
using Demo.Domain.Tests.Utility;
using FluentAssertions;
using Lucid.Persistence.Testing;
using NUnit.Framework;

namespace Demo.Domain.Tests.Account
{
    public class UserBuilder : Builder<User>
    {
        static UserBuilder()
        {
            DemoCustomInspections.Add<User>((validator, user) =>
            {
                validator.CheckNotNull(() => user.Email);
                validator.CheckMaxLength(() => user.Email, Constraints.DefaultMaxStringLength);
            });
        }

        public UserBuilder()
        {
            With(u => u.Email, "test.mail@test.site");
            With(u => u.LastLoggedIn, TestValues.EarlyDateTimeValue);
        }
    }

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

            var anotherUser = new UserBuilder().With(u => u.Email, "another@user.net").Save();

            var user = User.Login("existing@user.net");

            user.Should().BeSameAs(existingUser);
            user.LastLoggedIn.Should().Be(now);
        }
    }
}

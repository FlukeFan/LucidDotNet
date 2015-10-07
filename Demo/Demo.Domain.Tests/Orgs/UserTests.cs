using System;
using Demo.Domain.Orgs;
using FluentAssertions;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Orgs
{
    public class UserBuilder : Builder<User>
    {
        static UserBuilder()
        {
            LucidPersistenceValidator.AddCustomValidation<User>((validator, user) =>
            {
                validator.CheckNotNull(() => user.Email);
            });
        }

        public UserBuilder()
        {
            With(u => u.Email, "test.mail@test.site");
            With(u => u.LastLoggedIn, TestValues.EarlyDateTimeValue);
        }
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

            var anotherUser = new UserBuilder().With(u => u.Email, "another@user.net").Save();

            var user = User.Login("existing@user.net");

            user.Should().BeSameAs(existingUser);
            user.LastLoggedIn.Should().Be(now);
        }
    }
}

﻿using Lucid.Domain.Account;
using Lucid.Domain.Contract;
using Lucid.Domain.Tests.Account;
using Lucid.Infrastructure.Tests.Utility;
using FluentAssertions;
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
            loadedUser.Email.Should().Be(user.Email);
            loadedUser.LastLoggedIn.Should().Be(user.LastLoggedIn);
        }

        [Test]
        public void User_VerifyConstraints()
        {
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Email, null).Value());
            VerifyInvalidConstraint(new UserBuilder().With(d => d.Email, new string('x', Constraints.DefaultMaxStringLength + 1)).Value());
        }
    }
}

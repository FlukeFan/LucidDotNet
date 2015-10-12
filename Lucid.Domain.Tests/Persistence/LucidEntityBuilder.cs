using System;
using Lucid.Domain.Testing;

namespace Lucid.Domain.Tests.Persistence
{
    public class LucidEntityBuilder : Builder<LucidEntity>
    {
        public static readonly DateTime DefaultLastLoggedIn = new DateTime(2001, 02, 03, 04, 05, 06);

        public LucidEntityBuilder()
        {
            With(u => u.Email, "test.mail@test.site");
            With(u => u.LastLoggedIn, DefaultLastLoggedIn);
        }
    }
}

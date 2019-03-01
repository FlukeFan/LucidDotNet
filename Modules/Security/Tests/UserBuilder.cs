using Lucid.Infrastructure.Lib.Testing.Execution;
using Reposify.Testing;

namespace Lucid.Modules.Security.Tests
{
    public class UserBuilder : Builder<User>
    {
        static UserBuilder()
        {
            CustomChecks.Add<User>((cc, e) =>
            {
                cc.CheckNotNull(() => e.Name);
            });
        }

        public UserBuilder()
        {
            With(e => e.Name, "TestUser");
            With(e => e.LastLoggedIn, Defaults.SummerNow);
        }
    }
}

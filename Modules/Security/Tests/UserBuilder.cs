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
            With(e => e.LastLoggedIn, new System.DateTime(2008, 07, 06));
        }
    }
}

using Reposify.Testing;

namespace Lucid.Modules.Security.Tests
{
    public class UserBuilder : Builder<User>
    {
        static UserBuilder()
        {
            CustomChecks.Add<User>((cc, u) =>
            {
                cc.CheckNotNull(() => u.Name);
            });
        }

        public UserBuilder()
        {
            With(u => u.Name, "TestUser");
            With(u => u.LastLoggedIn, new System.DateTime(2008, 07, 06));
        }
    }
}

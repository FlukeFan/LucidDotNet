using Lucid.Lib.Facade.Pledge;

namespace Lucid.Modules.Security.Tests
{
    public class Agreements
    {
        public static Agreement<LoginCommand, User> Login =
            AgreementBuilder.For(() =>
                new LoginCommand
                {
                    UserName = "TestUser",
                })
            .Result(() =>
                new UserBuilder()
                    .Value());
    }
}

using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace Lucid.Modules.Temp.Tests
{
    public class TestStartup : AbstractTestStartup
    {
        public const string AuthCookieName = "TestStartupAuthCookie";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = Actions.Login();
                    o.Cookie.Name = AuthCookieName;
                });

            base.ConfigureServices(services);
        }
    }
}

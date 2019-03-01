using System.Collections.Generic;
using System.Security.Claims;
using Lucid.Infrastructure.Lib.MvcApp;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lucid.Infrastructure.Lib.Testing.Controller
{
    public class StubUserFilter : IActionFilter
    {
        public static User StubUser;

        public static void SetupDefault()
        {
            StubUser = new User
            {
                Id = Defaults.UserId,
                Name = "UnitTest",
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (StubUser == null)
            {
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
                return;
            };

            var claims = new[]
            {
                new Claim("Id", StubUser.Id.ToString()),
                new Claim(ClaimTypes.Name, StubUser.Name),
            };

            var identity = new AuthenticatedIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public class AuthenticatedIdentity : ClaimsIdentity
        {
            public AuthenticatedIdentity(IEnumerable<Claim> claims) : base(claims) { }

            public override bool IsAuthenticated => true;
        }
    }
}

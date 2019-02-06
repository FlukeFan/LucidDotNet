using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Account
{
    public static class Actions
    {
        internal const string RoutePrefix = "account";

        public static string Index()        { return $"/{RoutePrefix}"; }
        public static string LoginSuccess() { return $"/{RoutePrefix}/loginSuccess"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.Controller
    {
        [HttpGet]
        public Task<IActionResult> Index()
        {
            return RenderIndex(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login cmd)
        {
            return await ExecAsync(cmd,
                success: user => Login(user),
                failure: () => RenderIndex(cmd));
        }

        private Task<IActionResult> RenderIndex(Login cmd)
        {
            var model = new IndexModel { Cmd = cmd };
            return Task.FromResult<IActionResult>(View(model));
        }

        private async Task<IActionResult> Login(User user)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(Actions.LoginSuccess());
        }

        [HttpGet("loginSuccess")]
        public IActionResult LoginSuccess()
        {
            return View();
        }
    }
}

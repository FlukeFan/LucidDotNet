using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Security
{
    public static class Actions
    {
        internal const string RoutePrefix = "security";

        public static string Login()        { return $"/{RoutePrefix}/login"; }
        public static string LogOut()       { return $"/{RoutePrefix}/logOut"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.Controller
    {
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return RenderIndex(new LoginCommand());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand cmd)
        {
            return await ExecAsync(cmd,
                success: user => Login(user),
                failure: () => RenderIndex(cmd));
        }

        private IActionResult RenderIndex(LoginCommand cmd)
        {
            var model = new LoginModel { Cmd = cmd };
            return View(model);
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

            var returnUrl = Request.Query["returnUrl"];
            return Redirect(returnUrl.Count > 0 ? returnUrl[0] : "/");
        }

        [HttpGet("logOut")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}

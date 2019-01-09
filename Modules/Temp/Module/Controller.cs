using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp
{
    public static class Actions
    {
        internal const string RoutePrefix = "temp";

        public static string Index() { return $"/{RoutePrefix}"; }
        public static string Login() { return $"/{RoutePrefix}/login"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View(model);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var claims = new[]
            {
                new Claim("Id", "123"),
                new Claim(ClaimTypes.Name, "TemporaryUserName"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Content("Logged in");
        }
    }
}

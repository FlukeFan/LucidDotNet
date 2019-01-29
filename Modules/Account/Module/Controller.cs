using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Account
{
    public static class Actions
    {
        internal const string RoutePrefix = "account";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.AccountController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Render(new Login());
        }

        private IActionResult Render(Login cmd)
        {
            var model = new IndexModel
            {
                Cmd = cmd,
            };

            return View(model);
        }
    }
}

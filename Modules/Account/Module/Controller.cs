using System.Threading.Tasks;
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
        public IActionResult Index()
        {
            return RenderIndex(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login cmd)
        {
            return await ExecAsync(cmd,
                success: user => Login(user),
                failure: () => null);
        }

        private IActionResult RenderIndex(Login cmd)
        {
            var model = new IndexModel { Cmd = cmd };
            return View(model);
        }

        private IActionResult Login(User user)
        {
            return Redirect(Actions.LoginSuccess());
        }

        [HttpGet("loginSuccess")]
        public IActionResult LoginSuccess()
        {
            return View();
        }
    }
}

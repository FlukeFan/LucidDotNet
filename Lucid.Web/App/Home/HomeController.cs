using System.Web.Mvc;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Web.Utility;

namespace Lucid.Web.App.Home
{
    public static class Actions
    {
        public static string Index() { return "~/"; }
        public static string Login() { return "~/home/login/"; }
    }

    public class HomeController : LucidController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(Login cmd)
        {
            return Exec(cmd,
                success: r => Redirect(Actions.Index()),
                failure: () => View(new LoginModel { Cmd = cmd }));
        }
    }
}
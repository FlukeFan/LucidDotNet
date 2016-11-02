using System.Web.Mvc;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Domain.Contract.Account.Responses;
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
                success: r => Login(r),
                failure: () => View(new LoginModel { Cmd = cmd }));
        }

        private ActionResult Login(UserDto domainUser)
        {
            var lucidUser = new LucidUser(domainUser);
            CookieAuthentication.Authenticate(Response, lucidUser);
            return Redirect(Actions.Index());
        }
    }
}
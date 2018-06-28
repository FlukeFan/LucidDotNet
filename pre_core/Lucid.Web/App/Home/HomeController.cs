using System.Web;
using System.Web.Mvc;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Domain.Contract.Account.Responses;
using Lucid.Web.Utility;

namespace Lucid.Web.App.Home
{
    public static class Actions
    {
        public static string Index()                        { return "~/"; }
        public static string Login(string returnUrl = null) { return "~/home/login/" + ((returnUrl == null) ? "" : $"?returnUrl={HttpUtility.UrlEncode(returnUrl)}"); }
        public static string LogOut()                       { return "~/home/logout/"; }
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
            return Redirect(Request["returnUrl"] ?? Actions.Index());
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            CookieAuthentication.LogOut(Response);
            return Redirect(Actions.Index());
        }
    }
}
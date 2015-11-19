using System.Web.Mvc;
using Demo.Web.Utility;

namespace Demo.Web.App.Home
{
    public class HomeController : DemoController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
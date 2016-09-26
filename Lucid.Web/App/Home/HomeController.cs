using System.Web.Mvc;
using Demo.Web.Utility;

namespace Demo.Web.App.Home
{
    public class HomeController : DemoController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
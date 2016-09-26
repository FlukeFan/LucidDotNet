using System.Web.Mvc;
using Lucid.Web.Utility;

namespace Lucid.Web.App.Home
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
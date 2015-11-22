using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.Home
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("first controller");
        }
    }
}
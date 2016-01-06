using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.Home
{
    public class HomeController : Controller
    {
        public static string RootHomeControllerResponseText = "not set";

        public ActionResult Index()
        {
            return Content(RootHomeControllerResponseText);
        }

        [HttpPost]
        public ActionResult Post()
        {
            return Content("posted");
        }
    }
}
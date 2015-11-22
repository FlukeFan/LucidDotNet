using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.F1.Home
{
    public class HomeController : Controller
    {
        public ActionResult Other()
        {
            return Content("F1/Home/Other");
        }
    }
}
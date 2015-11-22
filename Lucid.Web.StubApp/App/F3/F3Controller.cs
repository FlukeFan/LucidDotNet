using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.F3
{
    public class F3Controller : Controller
    {
        public ActionResult Index()
        {
            return Content("F3/Index");
        }
    }
}
using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.F2
{
    public class F2Controller : Controller
    {
        public ActionResult Index()
        {
            return Content("F2/Index");
        }
    }
}
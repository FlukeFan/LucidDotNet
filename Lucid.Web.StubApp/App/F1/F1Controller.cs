using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.F1
{
    public class F1Controller : Controller
    {
        public ActionResult Index()
        {
            return Content("F1/Index");
        }
    }
}
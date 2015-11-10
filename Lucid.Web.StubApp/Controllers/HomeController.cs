using System.Web.Mvc;

namespace Lucid.Web.StubApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("first controller");
        }
    }
}
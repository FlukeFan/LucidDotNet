using System.Web.Mvc;

namespace Lucid.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("first controller");
        }
    }
}
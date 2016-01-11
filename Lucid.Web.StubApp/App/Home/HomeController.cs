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
            return Redirect("/otherAddress");
        }

        [HttpPost]
        public ActionResult PostValues()
        {
            var v1 = Request.Form["V1"];
            var v2 = Request.Form["%&V2"];
            return Content(string.Format("V1={0}\nV2={1}", v1, v2));
        }

        public ActionResult Return500()
        {
            throw new System.Exception("returns 500");
        }
    }
}
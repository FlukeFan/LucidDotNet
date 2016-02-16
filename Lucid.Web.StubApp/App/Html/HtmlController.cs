using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.Html
{
    public class HtmlController : Controller
    {
        [HttpGet]
        public ActionResult Component_Render()
        {
            return View();
        }
    }
}
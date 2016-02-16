using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.Html.Component
{
    public class ComponentController : Controller
    {
        [HttpGet]
        public ActionResult RenderComponent()
        {
            return View();
        }
    }
}
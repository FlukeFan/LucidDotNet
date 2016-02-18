using System.Web.Mvc;

namespace Lucid.Web.StubApp.App.Html
{
    public class HtmlController : Controller
    {
        [HttpGet]
        public ActionResult Component_Render() { return View(); }

        [HttpGet]
        public ActionResult Form_Render() { return View(); }

        [HttpGet]
        public ActionResult Form_RenderInput() { return View(new FormModel()); }
    }
}
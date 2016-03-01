using System.IO;
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

        public ActionResult RawForm()
        {
            var model = new RawFormModel();

            model.Url = Request.RawUrl;
            model.HttpMethod = Request.HttpMethod;
            model.ContentType = Request.ContentType;

            var requestInput = Request.InputStream;
            requestInput.Position = 0;
            using (var sr = new StreamReader(requestInput))
                model.Content = sr.ReadToEnd();

            return View(model);
        }
    }
}
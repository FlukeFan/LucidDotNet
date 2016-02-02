using System.Web.Mvc;
using Demo.Web.Utility;

namespace Demo.Web.App.Generation
{
    public static class Actions
    {
        public static string Generate() { return "~/Generation/Generate"; }
    }

    public class GenerationController : DemoController
    {
        public ActionResult Generate()
        {
            return View();
        }
    }
}
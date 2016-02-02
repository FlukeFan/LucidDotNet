using System.Web.Mvc;
using Demo.Web.ProjectCreation;
using Demo.Web.Utility;

namespace Demo.Web.App.Generation
{
    public static class Actions
    {
        public static string Generate() { return "~/Generation/Generate"; }
    }

    public class GenerationController : DemoController
    {
        [HttpGet]
        public ActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generate(GenerateProject cmd)
        {
            return Exec(cmd,
                success: r => File(r, "application/octet-stream", cmd.Name + ".zip"),
                failure: () => View());
        }
    }
}
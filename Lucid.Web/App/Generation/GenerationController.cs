using System.Web.Mvc;
using Lucid.Web.ProjectCreation;
using Lucid.Web.Utility;

namespace Lucid.Web.App.Generation
{
    public static class Actions
    {
        public static string Generate() { return "~/Generation/Generate"; }
    }

    public class GenerationController : LucidController
    {
        [HttpGet]
        public ActionResult Generate()
        {
            var initialCmd = new GenerateProject { Name = "Demo" };
            return GenerateRender(initialCmd);
        }

        [HttpPost]
        public ActionResult Generate(GenerateProject cmd)
        {
            return Exec(cmd,
                success: r => File(r, "application/octet-stream", cmd.Name + ".zip"),
                failure: () => GenerateRender(cmd));
        }

        private ActionResult GenerateRender(GenerateProject cmd)
        {
            var model = new GenerateModel { Cmd = cmd };
            return View(model);
        }
    }
}
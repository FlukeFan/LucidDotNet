using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    public static class Actions
    {
        public const string RoutePrefix = "projectCreation";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.ProjectCreationController
    {

        [HttpGet(Name = "GenerateProject")]
        public IActionResult Index()
        {
            return Render(new GenerateProject());
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexModel post)
        {
            SetFacadePropertyPrefix("Cmd.");

            return await Exec(post.Cmd,
                success: bytes => File(bytes, "application/zip", $"{post.Cmd.Name}.zip"),
                failure: () => Render(post.Cmd));
        }

        private IActionResult Render(GenerateProject cmd)
        {
            var model = new IndexModel
            {
                Now = DateTime.Now,
                Cmd = cmd,
            };

            return View(model);
        }
    }
}

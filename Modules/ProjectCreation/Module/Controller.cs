using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    public static class Actions
    {
        internal const string RoutePrefix = "projectCreation";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route(Actions.RoutePrefix)]
    [AllowAnonymous]
    public class Controller : Registry.ProjectCreationController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Render(new GenerateProject());
        }

        [HttpPost]
        public async Task<IActionResult> Index(GenerateProject cmd)
        {
            return await ExecAsync(cmd,
                success: bytes => File(bytes, "application/zip", $"{cmd.Name}.zip"),
                failure: () => Render(cmd));
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

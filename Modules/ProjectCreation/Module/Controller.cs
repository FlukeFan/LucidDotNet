using System;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    public static class Actions
    {
        public const string RoutePrefix = "projectCreation";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route("/")]
    public class RootController : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect(Actions.Index());
        }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : MvcAppController
    {

        [HttpGet(Name = "GenerateProject")]
        public IActionResult Index()
        {
            var model = new IndexModel
            {
                Now = DateTime.Now,
                Cmd = new GenerateProject(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexModel post)
        {
            var bytes = (byte[])await Registry.Executor.Execute(post.Cmd);
            return File(bytes, "application/zip", $"{post.Cmd.Name}.zip");
        }
    }
}

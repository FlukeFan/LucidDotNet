using System;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    [Route("/")]
    public class Controller : MvcAppController
    {
        [HttpGet("/")]
        public IActionResult Root()
        {
            return Redirect("projectCreation");
        }

        [HttpGet("projectCreation", Name = "GenerateProject")]
        public IActionResult Index()
        {
            var model = new IndexModel
            {
                Now = DateTime.Now,
                Cmd = new GenerateProject(),
            };

            return View(model);
        }

        [HttpPost("projectCreation")]
        public async Task<IActionResult> Index(IndexModel post)
        {
            var bytes = (byte[])await Registry.Executor.Execute(post.Cmd);
            return File(bytes, "application/zip", $"{post.Cmd.Name}.zip");
        }
    }
}

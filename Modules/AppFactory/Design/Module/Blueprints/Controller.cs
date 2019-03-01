using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;
using MvcForms;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public static class Actions
    {
        internal const string RoutePrefix = "appFactory/design/blueprints";

        public static string List()     { return $"/{RoutePrefix}/list"; }
        public static string Start()    { return $"/{RoutePrefix}/start"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var blueprints = ExecAsync(
                new FindBlueprintsQuery
                {
                    UserId = HttpContext.LoggedInUser().Id
                });

            var model = new ListModel
            {
                Blueprints = await blueprints,
            };

            return View(model);
        }

        [HttpGet("start")]
        public IActionResult Start()
        {
            return RenderStart(new StartCommand());
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start(StartCommand cmd)
        {
            cmd.OwnedByUserId = HttpContext.LoggedInUser().Id;

            return await ExecAsync(cmd,
                success: blueprint => this.ReturnModal(),
                failure: () => RenderStart(cmd));
        }

        private IActionResult RenderStart(StartCommand cmd)
        {
            var model = new StartModel { Cmd = cmd };
            return View(model);
        }
    }
}

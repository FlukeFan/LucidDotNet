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
        public static string Start()    { return $"/{RoutePrefix}/startEdit"; }
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

        [HttpGet("startEdit")]
        public IActionResult StartEdit()
        {
            return RenderStartEdit(new StartEditCommand());
        }

        [HttpPost("startEdit")]
        public async Task<IActionResult> StartEdit(StartEditCommand cmd)
        {
            cmd.OwnedByUserId = HttpContext.LoggedInUser().Id;

            return await ExecAsync(cmd,
                success: blueprint => this.ReturnModal(),
                failure: () => RenderStartEdit(cmd));
        }

        private IActionResult RenderStartEdit(StartEditCommand cmd)
        {
            var model = new StartEditModel
            {
                Cmd = cmd,
            };

            if (cmd.BlueprintId == 0)
            {
                model.Title = "Start Blueprint";
                model.ButtonText = "Start Blueprint";
            }
            else
            {
                model.Title = "Edit Blueprint";
                model.ButtonText = "Update";
            }

            return View(model);
        }
    }
}

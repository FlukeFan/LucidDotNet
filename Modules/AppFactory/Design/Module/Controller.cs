using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.AppFactory.Design
{
    public static class Actions
    {
        internal const string RoutePrefix = "appFactory/design";

        public static string List()     { return $"/{RoutePrefix}/list"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : Registry.Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var blueprints = ExecAsync(new FindBlueprintsQuery());

            var model = new ListModel
            {
                Blueprints = await blueprints,
            };

            return View(model);
        }
    }
}

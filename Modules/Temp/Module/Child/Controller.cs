using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp.Child
{
    public static class Actions
    {
        internal const string RoutePrefix = "temp/child";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

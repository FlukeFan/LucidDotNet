using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp.Child
{
    [Route(RouteUrl)]
    public class Controller : MvcAppController
    {
        public const string RouteUrl = "temp/child";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp.Child
{
    [Route(RouteUrl)]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string RouteUrl = "temp/child";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

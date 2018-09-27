using System;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp
{
    public abstract class LucidController : Microsoft.AspNetCore.Mvc.Controller
    {
    }

    [Route(RouteUrl)]
    public class Controller : LucidController
    {
        public const string RouteUrl = "temp/";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View("/Index.cshtml", model);
        }
    }
}

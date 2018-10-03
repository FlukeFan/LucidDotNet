using System;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp
{
    [Route(RouteUrl)]
    public class Controller : MvcAppController
    {
        public const string RouteUrl = "temp/";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View(model);
        }
    }
}

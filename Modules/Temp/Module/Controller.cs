using System;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.Temp
{
    public static class Actions
    {
        internal const string RoutePrefix = "temp";

        public static string Index() { return $"/{RoutePrefix}"; }
    }

    [Route(Actions.RoutePrefix)]
    public class Controller : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View(model);
        }
    }
}

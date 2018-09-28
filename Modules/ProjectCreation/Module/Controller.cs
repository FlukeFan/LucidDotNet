using System;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    [Route("/")]
    [Route(RouteUrl)]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string RouteUrl = "projectCreation/";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View(model);
        }
    }
}

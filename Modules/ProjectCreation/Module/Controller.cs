using System;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    public abstract class LucidController : Microsoft.AspNetCore.Mvc.Controller
    {
    }

    [Route("/")]
    [Route(RouteUrl)]
    public class Controller : LucidController
    {
        public const string RouteUrl = "projectCreation/";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel { Now = DateTime.Now };
            return View("/Index.cshtml", model);
        }
    }
}

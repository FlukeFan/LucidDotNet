using System;
using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Modules.ProjectCreation
{
    [Route("/")]
    [Route(RouteUrl, Name = "GenerateProject")]
    public class Controller : MvcAppController
    {
        public const string RouteUrl = "projectCreation/";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel
            {
                Now = DateTime.Now,
                Cmd = new GenerateProject(),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IndexModel post)
        {
            return View(post);
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace Lucid.ProjectCreation
{
    [Route("/projectCreation")]
    public class ProjectCreationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Hello from project creation");
        }
    }
}

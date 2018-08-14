using System;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.ProjectCreation
{
    public abstract class LucidController : Microsoft.AspNetCore.Mvc.Controller
    {
    }

    [Route("/")]
    [Route("/projectCreation")]
    public class Controller : LucidController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content($"Hello from deployed Lucid project creation - {DateTime.Now}");
        }
    }
}

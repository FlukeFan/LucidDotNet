﻿using Lucid.Infrastructure.Lib.MvcApp;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Infrastructure.Host.Web.Home
{
    [Route("/")]
    public class RootController : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
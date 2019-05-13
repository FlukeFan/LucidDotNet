using Lucid.Lib.MvcApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucid.Host.Web.Home
{
    [Route("/")]
    [AllowAnonymous]
    public class RootController : MvcAppController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

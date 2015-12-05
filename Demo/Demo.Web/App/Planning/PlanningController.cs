using System.Web.Mvc;
using Demo.Domain.Product.Commands;
using Demo.Web.Utility;

namespace Demo.Web.App.Planning
{
    public class PlanningController : DemoController
    {
        public ActionResult StartDesign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartDesign(StartDesign cmd)
        {
            return View();
        }
    }
}
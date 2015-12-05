using System.Web.Mvc;
using Demo.Web.Utility;

namespace Demo.Web.App.Planning
{
    public class PlanningController : DemoController
    {
        public ActionResult StartDesign()
        {
            return View();
        }
    }
}
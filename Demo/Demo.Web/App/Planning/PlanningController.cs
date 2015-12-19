using System;
using System.Web.Mvc;
using Demo.Domain.Product.Commands;
using Demo.Web.Utility;

namespace Demo.Web.App.Planning
{
    public static class Actions
    {
        public static string GetStartDesign() { return "~/Planning/StartDesign"; }
    }

    public class PlanningController : DemoController
    {
        public ActionResult StartDesign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartDesign(StartDesign cmd)
        {
            return Execute(cmd,
                success: r => View(),
                failure: () => { throw new Exception("failure not handled!"); });
        }
    }
}
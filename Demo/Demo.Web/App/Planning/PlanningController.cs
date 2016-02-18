using System.Web.Mvc;
using Demo.Domain.Contract.Product.Commands;
using Demo.Domain.Contract.Product.Queries;
using Demo.Web.Utility;

namespace Demo.Web.App.Planning
{
    public static class Actions
    {
        public static string List()         { return "~/Planning/List"; }
        public static string StartDesign()  { return "~/Planning/StartDesign"; }
    }

    public class PlanningController : DemoController
    {
        [HttpGet]
        public ActionResult List()
        {
            var designs = Exec(new FindDesigns());

            return View(designs);
        }

        [HttpGet]
        public ActionResult StartDesign()
        {
            var initialCmd = new StartDesign();
            return StartDesign_Render(initialCmd);
        }

        [HttpPost]
        public ActionResult StartDesign(StartDesign cmd)
        {
            return Exec(cmd,
                success: r => Redirect(Actions.List()),
                failure: () => StartDesign_Render(cmd));
        }

        private ActionResult StartDesign_Render(StartDesign cmd)
        {
            var model = new StartDesignModel { Cmd = cmd };
            return View(model);
        }
    }
}
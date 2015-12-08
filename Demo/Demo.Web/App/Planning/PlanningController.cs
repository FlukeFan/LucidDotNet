using System.Web.Mvc;
using Demo.Domain.Product.Commands;
using Demo.Infrastructure.NHibernate;
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
            if (!Lucid.Web.WebHost.IsRunningInTestHost)
            {
                using (var repository = new DemoNhRepository())
                {
                    repository.Open();
                    Domain.Utility.Registry.Repository = repository;
                    cmd.Execute();
                    repository.Commit();
                }
            }

            return View();
        }
    }
}
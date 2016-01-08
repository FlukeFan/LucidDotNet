using System;
using System.Web.Mvc;
using Demo.Infrastructure.NHibernate;
using Lucid.Domain.Execution;

namespace Demo.Web.Utility
{
    public class MvcExecutor
    {
        public virtual ActionResult Execute<T>(Command<T> cmd, Func<T, ActionResult> success, Func<ActionResult> failure)
        {
            using (var repository = new DemoNhRepository())
            {
                repository.Open();
                Domain.Utility.Registry.Repository = repository;
                var response = cmd.Execute();
                repository.Commit();
                return success(response);
            }
        }
    }
}
using System;
using System.Web.Mvc;
using SimpleFacade;
using SimpleFacade.Exceptions;
using SimpleFacade.Execution;

namespace Lucid.Web.Utility
{
    public abstract class DemoController : Controller
    {
        protected TReturn Exec<TReturn>(IQuery<TReturn> query)
        {
            return Executor().Execute(query);
        }

        protected ActionResult Exec(ICommand cmd, Func<ActionResult> success, Func<ActionResult> failure)
        {
            return Exec<object>(ModelState, () => { Executor().Execute(cmd); return null; }, nullValue => success(), failure);
        }

        protected ActionResult Exec<TReturn>(ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            return Exec(ModelState, () => Executor().Execute(cmd), success, failure);
        }

        private static ICqExecutor Executor()
        {
            return PresentationRegistry.Executor;
        }

        private static ActionResult Exec<TReturn>(ModelStateDictionary modelState, Func<TReturn> run, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            TReturn response = default(TReturn);

            if (IsValid(modelState))
            {
                try
                {
                    response = run();
                }
                catch (FacadeException exception)
                {
                    AddErrors(modelState, exception);
                }
            }

            return IsValid(modelState)
                ? success(response)
                : failure();
        }

        private static bool IsValid(ModelStateDictionary modelState)
        {
            lock (modelState)
                return modelState.IsValid;
        }

        private static void AddErrors(ModelStateDictionary modelState, FacadeException exception)
        {
            lock (modelState)
            {
                foreach (var message in exception.Messages)
                    modelState.AddModelError("", message);

                foreach (var kvp in exception.PropertyMessages)
                    foreach (var message in kvp.Value)
                        modelState.AddModelError(kvp.Key, message);
            }
        }
    }
}
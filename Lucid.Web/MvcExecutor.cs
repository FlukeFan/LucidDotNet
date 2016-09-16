﻿using System;
using System.Web.Mvc;
using SimpleFacade;
using SimpleFacade.Exceptions;
using SimpleFacade.Execution;

namespace Lucid.Web
{
    public static class MvcExecutor
    {
        public static TReturn Exec<TReturn>(ICqExecutor executor, IQuery<TReturn> query)
        {
            return executor.Execute(query);
        }

        public static ActionResult Exec(ModelStateDictionary modelState, ICqExecutor executor, ICommand cmd, Func<ActionResult> success, Func<ActionResult> failure)
        {
            return Exec<object>(modelState, () => { executor.Execute(cmd); return null; }, nullValue => success(), failure);
        }

        public static ActionResult Exec<TReturn>(ModelStateDictionary modelState, ICqExecutor executor, ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            return Exec(modelState, () => executor.Execute(cmd), success, failure);
        }

        public static ActionResult Exec<TReturn>(ModelStateDictionary modelState, Func<TReturn> run, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            TReturn response = default(TReturn);

            if (modelState.IsValid)
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

            return modelState.IsValid
                ? success(response)
                : failure();
        }

        public static void AddErrors(ModelStateDictionary modelState, FacadeException exception)
        {
            lock(modelState)
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

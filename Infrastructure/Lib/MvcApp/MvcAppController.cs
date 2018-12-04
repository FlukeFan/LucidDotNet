using System;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public abstract class MvcAppController : Controller
    {
        private string _facadePropertyPrefix;

        protected virtual IExecutor Executor()
        {
            throw new NotImplementedException("Need to override Executor()");
        }

        protected async Task<IActionResult> Exec<TResult>(ICommand<TResult> cmd, Func<TResult, IActionResult> success, Func<IActionResult> failure)
        {
            return await Exec<TResult>(ModelState, () => Executor().Execute(cmd), success, failure);
        }

        protected void SetFacadePropertyPrefix(string prefix)
        {
            _facadePropertyPrefix = prefix;
        }

        private async Task<IActionResult> Exec<TResult>(ModelStateDictionary modelState, Func<Task<object>> run, Func<TResult, IActionResult> success, Func<IActionResult> failure)
        {
            TResult response = default(TResult);

            if (modelState.IsValid)
            {
                try
                {
                    var objectResult = await run();
                    response = (TResult)objectResult;
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

        private void AddErrors(ModelStateDictionary modelState, FacadeException exception)
        {
            foreach (var message in exception.Messages)
                modelState.AddModelError("", message);

            foreach (var kvp in exception.PropertyMessages)
                foreach (var message in kvp.Value)
                    modelState.AddModelError($"{_facadePropertyPrefix}{kvp.Key}", message);
        }
    }
}

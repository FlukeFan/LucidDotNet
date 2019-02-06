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

        protected virtual IExecutorAsync ExecutorAsync()
        {
            throw new NotImplementedException("Need to override ExecutorAsync()");
        }

        protected async Task<IActionResult> ExecAsync<TResult>(ICommand<TResult> cmd, Func<TResult, Task<IActionResult>> success, Func<Task<IActionResult>> failure)
        {
            var context = new ExecutionContext { Executable = cmd };
            return await Exec<TResult>(ModelState, () => ExecutorAsync().ExecuteAsync(context), success, failure);
        }

        protected async Task<IActionResult> ExecAsync<TResult>(ICommand<TResult> cmd, Func<TResult, Task<IActionResult>> success, Func<IActionResult> failure)
        {
            var context = new ExecutionContext { Executable = cmd };
            return await Exec<TResult>(ModelState, () => ExecutorAsync().ExecuteAsync(context), success, () => Task.FromResult(failure()));
        }

        protected async Task<IActionResult> ExecAsync<TResult>(ICommand<TResult> cmd, Func<TResult, IActionResult> success, Func<Task<IActionResult>> failure)
        {
            var context = new ExecutionContext { Executable = cmd };
            return await Exec<TResult>(ModelState, () => ExecutorAsync().ExecuteAsync(context), r => Task.FromResult(success(r)), failure);
        }

        protected async Task<IActionResult> ExecAsync<TResult>(ICommand<TResult> cmd, Func<TResult, IActionResult> success, Func<IActionResult> failure)
        {
            var context = new ExecutionContext { Executable = cmd };
            return await Exec<TResult>(ModelState, () => ExecutorAsync().ExecuteAsync(context), r => Task.FromResult(success(r)), () => Task.FromResult(failure()));
        }

        protected void SetFacadePropertyPrefix(string prefix)
        {
            _facadePropertyPrefix = prefix;
        }

        private async Task<IActionResult> Exec<TResult>(ModelStateDictionary modelState, Func<Task<object>> run, Func<TResult, Task<IActionResult>> success, Func<Task<IActionResult>> failure)
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
                ? await success(response)
                : await failure();
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

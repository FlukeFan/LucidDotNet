using System.Threading.Tasks;
using Lucid.Lib.Facade;
using Lucid.Modules.AppFactory.Manufacturing.Domain.Cycles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lucid.Modules.AppFactory.Manufacturing.Pages.Cycles
{
    public class StartCycleModel : PageModel
    {
        // POST
        public StartCycleCommand Cmd;

        public void OnGet()
        {
            Cmd = new StartCycleCommand { BlueprintId = -1 };
        }

        public async Task<IActionResult> OnPostAsync(StartCycleCommand cmd)
        {
            await Registry.ExecutorAsync.ExecuteAsync(new ExecutionContext
            {
                Executable = cmd,
            });

            return Redirect("Index");
        }
    }
}

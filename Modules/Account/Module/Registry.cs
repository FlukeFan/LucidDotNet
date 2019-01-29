using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Validation;
using Lucid.Infrastructure.Lib.MvcApp;

namespace Lucid.Modules.Account
{
    public static class Registry
    {
        public static IExecutor Executor { get; set; }

        public static async Task StartupAsync()
        {
            await Task.Run(() =>
            {
                Executor =
                    new ValidatingExecutor(
                        new Executor());
            });
        }

        public class AccountController : MvcAppController
        {
            protected override IExecutor Executor() { return Registry.Executor; }
        }
    }
}

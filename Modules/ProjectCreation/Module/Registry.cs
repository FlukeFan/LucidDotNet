using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.ProjectCreation
{
    public static class Registry
    {
        public static IExecutor Executor { get; set; }

        public static async Task StartupAsync()
        {
            await Task.Run(() =>
            {
                Executor = new Executor();
            });
        }
    }
}

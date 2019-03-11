using System;
using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Util
{
    public static class TaskExtensions
    {
        public static async Task Then<T>(this Task<T> task, Action<T> then)
        {
            await task;
            then(task.Result);
        }
    }
}

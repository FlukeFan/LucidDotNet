using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public static class UtilityExtensions
    {
        public static TExecutable Mutate<TExecutable>(this TExecutable target, Action<TExecutable> mutator)
        {
            mutator(target);
            return target;
        }
    }
}

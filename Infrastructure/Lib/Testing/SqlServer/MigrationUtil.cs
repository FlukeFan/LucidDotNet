using FluentMigrator;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Options;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class MigrationUtil
    {
        public class OptionsSnapshot
        {
            public static IOptionsSnapshot<T> Create<T>(T options) where T : class, new()
            {
                return new OptionsSnapshot<T>(options);
            }
        }

        public class OptionsSnapshot<T> : OptionsWrapper<T>, IOptionsSnapshot<T> where T : class, new()
        {
            public OptionsSnapshot(T options) : base(options) { }
        }

        public class ProcessorAccessor : IProcessorAccessor
        {
            public IMigrationProcessor Processor { get; set; }

            public static ProcessorAccessor Create(IMigrationProcessor processor)
            {
                return new ProcessorAccessor { Processor = processor };
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Lucid.Lib.Facade;

namespace Lucid.Modules.AppFactory.Manufacturing.Domain.Cycles
{
    public class StartCycleCommand : CommandAsync<int>
    {
        public int  BlueprintId { get; set; }
        public int  Quantity    { get; set; }

        public override Task<int> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}

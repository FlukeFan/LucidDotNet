using System.Collections.Generic;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design.Contract
{
    public class FindAvailableBlueprints : IQuery<List<BlueprintDto>>
    {
        public int UserId;
    }
}

using System.Collections.Generic;
using Lucid.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design.Contract
{
    public class FindAvailableBlueprints : IQuery<List<BlueprintDto>>
    {
        public int UserId;
    }
}

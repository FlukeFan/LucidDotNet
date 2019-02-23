using System;

namespace Lucid.Modules.ProjectCreation
{
    public class IndexModel
    {
        public DateTime Now;

        // POST
        public GenerateProjectCommand Cmd { get; set; }
    }
}

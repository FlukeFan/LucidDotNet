﻿using Lucid.Infrastructure.Lib.Testing.Execution;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class Agreements
    {
        public static Agreement<GenerateProjectCommand> GenerateProject =
            AgreementBuilder.For(() =>
                new GenerateProjectCommand
                {
                    Name = "NewProj_1",
                })
            .NoResultDefined();
    }
}

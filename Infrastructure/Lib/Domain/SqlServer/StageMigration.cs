using System;
using FluentMigrator;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public abstract class StageMigration : Migration
    {
        public override void Down() { throw new NotImplementedException(); }
    }
}

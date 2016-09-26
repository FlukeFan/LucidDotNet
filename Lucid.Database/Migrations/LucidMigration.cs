using System;
using FluentMigrator;

namespace Lucid.Database.Migrations
{
    public abstract class LucidMigration : Migration
    {
        public override void Down() { throw new NotImplementedException(); }
    }
}

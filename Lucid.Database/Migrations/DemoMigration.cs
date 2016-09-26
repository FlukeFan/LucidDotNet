using System;
using FluentMigrator;

namespace Demo.Database.Migrations
{
    public abstract class DemoMigration : Migration
    {
        public override void Down() { throw new NotImplementedException(); }
    }
}

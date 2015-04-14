using System;
using FluentMigrator;

namespace LucidDotNet.Database.Migrations
{
    public abstract class LucidMigration : Migration
    {
        public override void Down() { throw new NotImplementedException(); }
    }
}

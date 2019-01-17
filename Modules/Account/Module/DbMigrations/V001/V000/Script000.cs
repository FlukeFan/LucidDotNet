﻿using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Account.DbMigrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, script: 0)]
    public class Script000 : SqlServerMigration
    {
        public override void Up()
        {
            Create.Table("User")
                .LucidPrimaryKey("User", "Id")
                .WithColumn("Name").AsAnsiString().NotNullable()
                .WithColumn("LastLoggedIn").AsDateTime().NotNullable();
        }
    }
}
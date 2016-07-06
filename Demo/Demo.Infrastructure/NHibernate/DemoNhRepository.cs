﻿using Demo.Domain.Utility;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Reposify.NHibernate;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
        public static void Init(string connection)
        {
            var rootEntityType = typeof(DemoEntity);

            var config = NhHelper.CreateConfig<int>(rootEntityType, cfg =>
            {
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = connection;
                    db.ConnectionProvider<DriverConnectionProvider>();
                    db.Driver<SqlClientDriver>();
                    db.Dialect<MsSql2008Dialect>();
                });
            });

            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            Init(config);
        }
    }
}

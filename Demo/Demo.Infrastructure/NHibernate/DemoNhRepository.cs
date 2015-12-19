using System;
using Demo.Domain.Utility;
using Lucid.Persistence.NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
        public static void Init(string connection, Type rootEntityType)
        {
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

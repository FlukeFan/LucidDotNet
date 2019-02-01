using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using Reposify.NHibernate;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class NhUtil
    {
        public static ISessionFactory CreateNhSessionFactory<TRootEntity>(string connectionString)
        {
            var mapper = new ConventionModelMapper();
            var mappings = NhHelper.CreateConventionalMappings<TRootEntity>(mapper);
            var config = NhHelper.CreateConfig(mappings, cfg =>
            {
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = connectionString;
                    db.ConnectionProvider<DriverConnectionProvider>();
                    db.Driver<SqlClientDriver>();
                    db.Dialect<MsSql2012Dialect>();
                    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                });
            });

            var sessionFactory = config.BuildSessionFactory();
            return sessionFactory;
        }
    }
}

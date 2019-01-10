using System;
using System.Data.SqlClient;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfig
    {
        protected virtual string MasterConnectionString()   { throw new Exception($"Override to create master connnection string for {GetType()}"); }
        protected virtual string DbName()                   { return "Lucid"; }

        public virtual void CreateDb()
        {
            var connectionString = MasterConnectionString();

            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();
                var cmd = cn.CreateCommand();
                cmd.CommandText = $"SELECT count(1) FROM master.dbo.sysdatabases WHERE name = N'{DbName()}'";
                var count = (int)cmd.ExecuteScalar();

                var dbExists = count > 0;

                if (dbExists)
                    return;

                cmd.CommandText = $"CREATE DATABASE {DbName()}";
                cmd.ExecuteNonQuery();
            }
        }
    }
}

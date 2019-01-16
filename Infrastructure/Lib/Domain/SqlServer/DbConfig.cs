using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfig
    {
        protected virtual string MasterConnectionString()               { throw new Exception($"Override to create master connnection string for {GetType()}"); }
        protected virtual string SchemaConnectionString(string name)    { throw new Exception($"Override to create schema connnection string for {GetType()}"); }
        protected virtual string DbName()                               { return "Lucid"; }
        protected virtual bool   CanCreateDb()                          { return true; }

        public virtual void CreateDb(params Schema[] schemas)
        {
            if (CanCreateDb())
            {
                var connectionString = MasterConnectionString();

                using (var cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    CreateDb(cn);

                    CreateSchemas(cn, schemas);
                }
            }
        }

        private void CreateDb(SqlConnection cn)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT count(1) FROM master.dbo.sysdatabases WHERE name = N'{DbName()}'";
                var count = (int)cmd.ExecuteScalar();

                var dbExists = count > 0;

                if (!dbExists)
                {
                    cmd.CommandText = $"CREATE DATABASE {DbName()}";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"USE {DbName()}";
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateSchemas(SqlConnection cn, Schema[] schemas)
        {
            using (var cmd = cn.CreateCommand())
            {
                var existingSchemas = SelectString(cmd, $"SELECT name FROM sys.schemas");
                var existingLogins = SelectString(cmd, $"SELECT name FROM sys.sql_logins");

                foreach (var schema in schemas)
                    CreateSchema(cmd, existingSchemas, existingLogins, schema);
            }
        }

        private void CreateSchema(SqlCommand cmd, IList<string> existingSchemas, IList<string> existingLogins, Schema schema)
        {
            if (!existingSchemas.Contains(schema.Name.ToLower()))
            {
                var login = $"{DbName()}_{schema.Name}";

                if (existingLogins.Contains(login.ToLower()))
                {
                    cmd.CommandText = $"DROP LOGIN [{login}]";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"CREATE LOGIN[{login}] WITH PASSWORD = N'Password12!', DEFAULT_DATABASE =[{DbName()}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE USER[{login}] FOR LOGIN[{login}] WITH DEFAULT_SCHEMA =[{schema.Name}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE SCHEMA[{schema.Name}] AUTHORIZATION[{login}]";
                cmd.ExecuteNonQuery();
            }

            schema.ConnectionString = SchemaConnectionString(schema.Name);
        }

        private IList<string> SelectString(SqlCommand cmd, string sql)
        {
            var items = new List<string>();
            cmd.CommandText = sql;

            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    items.Add(((string)reader[0]).ToLower());

            return items;
        }
    }
}

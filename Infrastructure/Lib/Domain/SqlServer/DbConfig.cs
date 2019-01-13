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
                cmd.CommandText = $"SELECT name FROM sys.schemas";
                var existingSchemas = new List<string>();

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        existingSchemas.Add(((string)reader[0]).ToLower());

                foreach (var schema in schemas)
                    CreateSchema(cmd, existingSchemas, schema);
            }
        }

        private void CreateSchema(SqlCommand cmd, IList<string> existingSchemas, Schema schema)
        {
            if (!existingSchemas.Contains(schema.Name.ToLower()))
            {
                cmd.CommandText = $"CREATE LOGIN[{DbName()}_{schema.Name}] WITH PASSWORD = N'Password12!', DEFAULT_DATABASE =[{DbName()}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE USER[{DbName()}_{schema.Name}] FOR LOGIN[{DbName()}_{schema.Name}] WITH DEFAULT_SCHEMA =[{schema.Name}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE SCHEMA[{schema.Name}] AUTHORIZATION[{DbName()}_{schema.Name}]";
                cmd.ExecuteNonQuery();
            }

            schema.ConnectionString = SchemaConnectionString(schema.Name);
        }
    }
}

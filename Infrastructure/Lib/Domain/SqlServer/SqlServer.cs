using System.Collections.Generic;
using System.Data.SqlClient;
using Lucid.Infrastructure.Lib.Util;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class SqlServer
    {
        private string _server;
        private string _dbName;
        private string _userId;
        private string _password;

        public SqlServer(string server, string dbName, string userId, string password)
        {
            _server = server;
            _dbName = dbName;
            _userId = userId;
            _password = password;
        }

        public void SetSchemaConnections(params Schema[] schemas)
        {
            foreach (var schema in schemas)
                schema.ConnectionString = $"Server={_server};Database={_dbName};User ID={_dbName}_{schema.Name};Password={_password}";
        }

        public void CreateSchemas(bool createDb, params Schema[] schemas)
        {
            using (new GlobalLock($"SetupSchemas_{_dbName}"))
            {
                var connectionString = createDb
                    ? $"Server={_server};Database=master;User ID={_userId};Password={_password}"
                    : $"Server={_server};Database={_dbName};User ID={_userId};Password={_password}";

                using (var cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    if (createDb)
                        CreateDb(cn);

                    CreateSchemas(cn, schemas);
                }
            }
        }

        private void CreateDb(SqlConnection cn)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT count(1) FROM master.dbo.sysdatabases WHERE name = N'{_dbName}'";
                var count = (int)cmd.ExecuteScalar();

                var dbExists = count > 0;

                if (!dbExists)
                {
                    cmd.CommandText = $"CREATE DATABASE {_dbName}";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"USE {_dbName}";
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
                var login = $"{_dbName}_{schema.Name}";

                if (existingLogins.Contains(login.ToLower()))
                {
                    cmd.CommandText = $"DROP LOGIN [{login}]";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"CREATE LOGIN[{login}] WITH PASSWORD = N'{_password}', DEFAULT_DATABASE =[{_dbName}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE USER[{login}] FOR LOGIN[{login}] WITH DEFAULT_SCHEMA =[{schema.Name}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"CREATE SCHEMA[{schema.Name}] AUTHORIZATION[{login}]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"GRANT CREATE TABLE TO [{login}]";
                cmd.ExecuteNonQuery();
            }
        }

        private static IList<string> SelectString(SqlCommand cmd, string sql)
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

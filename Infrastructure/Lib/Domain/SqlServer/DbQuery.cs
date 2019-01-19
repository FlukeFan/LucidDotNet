using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbQuery : IDisposable
    {
        private string              _connectionString;
        private Lazy<SqlConnection> _cn;

        public DbQuery(string connectionString, string schemaName)
        {
            _connectionString = connectionString;
            SchemaName = schemaName;

            _cn = new Lazy<SqlConnection>(() =>
            {
                var cn = new SqlConnection(_connectionString);
                cn.Open();
                return cn;
            });
        }

        public string SchemaName { get; }

        public IList<T> ExecuteList<T>(string sql)
        {
            var list = new List<T>();

            var cmd = _cn.Value.CreateCommand();
            cmd.CommandText = sql;

            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    list.Add((T)reader.GetValue(0));

            return list;
        }

        void IDisposable.Dispose()
        {
            if (_cn.IsValueCreated)
                using (_cn.Value) { }
        }
    }
}

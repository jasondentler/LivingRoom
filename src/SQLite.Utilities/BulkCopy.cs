using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace SQLite.Utilities
{
    public class BulkCopy
    {

        private const string AttachName = "asldkqwer";

        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;

        public BulkCopy(Configuration configuration, ISessionFactory sessionFactory)
        {
            _configuration = configuration;
            _sessionFactory = sessionFactory;
        }

        public void Export(string destinationFileName, bool clear)
        {
            if (clear)
                File.WriteAllBytes(destinationFileName, new byte[] { });

            if (!File.Exists(destinationFileName) || clear)
                CreateSchema(destinationFileName);

            using (var session = _sessionFactory.OpenSession())
            {
                var memory = (DbConnection)session.Connection;
                if (memory.State == ConnectionState.Closed)
                    memory.Open();
                var tableNames = GetTableNames(memory);
                AttachDatabase(memory, destinationFileName);
                foreach (var tableName in tableNames)
                    CopyTableData(memory, tableName, string.Format("{0}.{1}", AttachName, tableName));
                DetachDatabase(memory);
            }
        }

        public void Import(string sourceFileName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var memory = (DbConnection)session.Connection;
                if (memory.State == ConnectionState.Closed)
                    memory.Open();
                var tableNames = GetTableNames(memory);
                AttachDatabase(memory, sourceFileName);
                foreach (var tableName in tableNames)
                    CopyTableData(memory, string.Format("{0}.{1}", AttachName, tableName), tableName);
                DetachDatabase(memory);
            }
        }

        private void CreateSchema(string fileName)
        {
            var se = new SchemaExport(_configuration);
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = string.Format("Data Source={0};Version=3;New=True;", fileName);
                conn.Open();
                se.Execute(false, true, false, conn, null);
                conn.Close();
            }
        }

        private static void AttachDatabase(IDbConnection memory, string fileDbPath)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("ATTACH '{0}' AS {1}", fileDbPath, AttachName);
            cmd.ExecuteNonQuery();
        }

        private static IEnumerable<string> GetTableNames(DbConnection memory)
        {
            var dt = memory.GetSchema("Tables");
            return from DataRow row in dt.Rows
                   select (string) row["TABLE_NAME"];
        }

        private static void CopyTableData(IDbConnection memory, string source, string destination)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}",
                                            destination, source);
            cmd.ExecuteNonQuery();
        }

        private static void DetachDatabase(IDbConnection memory)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("DETACH {0}", AttachName);
            cmd.ExecuteNonQuery();
        }


    }
}

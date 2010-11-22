using System;
using System.Data;
using NHibernate.Connection;

namespace SQLite.Utilities
{
    public class PersistentConnectionProvider : DriverConnectionProvider 
    {

        [ThreadStatic]
        private static IDbConnection _connection;

        public static void CloseDatabase()
        {
            if (_connection != null)
                _connection.Dispose();
            _connection = null;
        }

        public override IDbConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = Driver.CreateConnection();
                _connection.ConnectionString = ConnectionString;
                _connection.Open();
            }
            return _connection;
        }

        public override void CloseConnection(IDbConnection conn)
        {
            // Do nothing
        }


    }
}

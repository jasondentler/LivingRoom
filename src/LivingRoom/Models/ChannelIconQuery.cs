using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace LivingRoom.Models
{
    public class ChannelIconQuery
    {


        public static string Query(string channelId)
        {
            string retval = "";
            using (var conn = Open())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IconUrl FROM Channel WHERE Id = @id LIMIT 1";
                    var idParam = cmd.CreateParameter();
                    idParam.DbType = DbType.AnsiString;
                    idParam.ParameterName = "id";
                    idParam.Value = channelId;
                    cmd.Parameters.Add(idParam);
                    retval = (string) cmd.ExecuteScalar();
                }
                conn.Close();
            }
            return retval;
        }

        private static IDbConnection Open()
        {
            var connStr = ConfigurationManager.ConnectionStrings["conn"];
            var factory = DbProviderFactories.GetFactory(connStr.ProviderName);
            var conn = factory.CreateConnection();
            conn.ConnectionString = connStr.ConnectionString;
            conn.Open();
            return conn;
        }

    }
}
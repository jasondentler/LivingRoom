using System.Configuration;
using System.Data.Common;
using System.IO;
using LivingRoom.XmlTv;

namespace ImportXmlTv
{
    class Program
    {
        private const string ScheduleFile = @"C:\XmlTv\Schedule.xml";

        static void Main(string[] args)
        {
            var iconFolderPath = ConfigurationManager.AppSettings["iconPath"];
            var dbFile = ConfigurationManager.AppSettings["dbPath"];
            if (File.Exists(dbFile))
                File.Delete(dbFile);

            var connStr = ConfigurationManager.ConnectionStrings["conn"];
            var factory = DbProviderFactories.GetFactory(connStr.ProviderName);
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = connStr.ConnectionString;
                conn.Open();
                Database.CreateSchema(conn);
                Schedule.Import(ScheduleFile, conn, iconFolderPath);
                Database.ExportToFile(conn, dbFile);
                conn.Close();
            }
        }
    }
}

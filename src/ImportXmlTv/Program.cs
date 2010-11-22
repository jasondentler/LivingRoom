using System;
using System.Configuration;
using System.IO;
using LivingRoom.XmlTv;
using NHibernate.Tool.hbm2ddl;
using SQLite.Utilities;

namespace ImportXmlTv
{
    class Program
    {
        private const string ScheduleFile = @"C:\XmlTv\Schedule.xml";

        static void Main(string[] args)
        {
            var iconFolderPath = ConfigurationManager.AppSettings["iconPath"];
            var dbFile = ConfigurationManager.AppSettings["dbPath"];

            var cfg = new NHibernate.Cfg.Configuration().Configure();
            var sessionFactory = cfg.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    Console.WriteLine("Building database tables");
                    new SchemaExport(cfg).Execute(false, true, false, session.Connection, null);
                    tx.Commit();
                }
                Console.WriteLine("Parsing XML in to memory database");
                new Schedule(ScheduleFile, iconFolderPath).Export(session);
            }
            Console.WriteLine("Saving memory database to disk");

            var tmpFile = Path.GetTempFileName();
            new BulkCopy(cfg, sessionFactory).Export(tmpFile, true);
            PersistentConnectionProvider.CloseDatabase();
            Console.WriteLine("Copying from temporary disk location to production.");
            File.Copy(tmpFile, dbFile, true);
            File.Delete(tmpFile);
            Console.WriteLine("All done. Press any key.");
            Console.ReadKey();

        }
    }
}

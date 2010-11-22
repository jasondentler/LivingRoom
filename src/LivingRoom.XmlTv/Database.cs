using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace LivingRoom.XmlTv
{
    public static class Database
    {

        public static void CreateSchema(string connectionString, string providerName)
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                CreateSchema(conn);
                conn.Close();
            }
        }

        public static void CreateSchema(IDbConnection connection)
        {
            var cmdStrings = new string[]
                                 {
                                     @"CREATE TABLE Channel (Id TEXT PRIMARY KEY, Channel integer, ShortName TEXT, LongName TEXT, IconURL TEXT)"
                                     ,
                                     @"CREATE TABLE Program (Id TEXT PRIMARY KEY, ChannelId, StartTime datetime, EndTime datetime, Title, Description, EpisodeId, EpisodeNumber)"
                                     ,
                                     @"CREATE TABLE ProgramCategories (Id, Category)"
                                     ,
                                     @"CREATE TABLE ProgramCredits (Id, Role, Name)"
                                 };
            foreach (var cmdStr in cmdStrings)
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = cmdStr;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void WriteChannel(IDbConnection connection,
                                         string id, string channel, 
                                         string shortName, string longName,
                                         string icon)
        {
            const string cmdStr = @"INSERT INTO Channel (Id, Channel, ShortName, LongName, IconUrl) VALUES (@id, @channel, @shortName, @longName, @iconUrl)";
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = cmdStr;
                cmd.CommandType = CommandType.Text;

                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "id";
                idParam.DbType = DbType.AnsiString;
                idParam.Value = (object) id ?? DBNull.Value;
                cmd.Parameters.Add(idParam);
                
                var channelParam = cmd.CreateParameter();
                channelParam.ParameterName = "channel";
                channelParam.DbType = DbType.AnsiString;
                channelParam.Value = (object)channel ?? DBNull.Value;
                cmd.Parameters.Add(channelParam);

                var shortNameParam = cmd.CreateParameter();
                shortNameParam.ParameterName = "shortName";
                shortNameParam.DbType = DbType.AnsiString;
                shortNameParam.Value = (object)shortName ?? DBNull.Value;
                cmd.Parameters.Add(shortNameParam);

                var longNameParam = cmd.CreateParameter();
                longNameParam.ParameterName = "longName";
                longNameParam.DbType = DbType.AnsiString;
                longNameParam.Value = (object)longName ?? DBNull.Value;
                cmd.Parameters.Add(longNameParam);

                var iconParam = cmd.CreateParameter();
                iconParam.ParameterName = "iconUrl";
                iconParam.DbType = DbType.AnsiString;
                iconParam.Value = (object)icon ?? DBNull.Value;
                cmd.Parameters.Add(iconParam);

                cmd.ExecuteNonQuery();
            }
        }

        public static void WriteProgram(IDbConnection connection, Guid id, 
                                         string channel, DateTime startTime, DateTime endTime, 
                                         string title, string description, string episodeId, string episodeNum)
        {
            //Id, ChannelId, StartTime, EndTime, Title, 
            // Description, EpisodeId, EpisodeNumber

            // const string cmdStr =
            //@"INSERT INTO Program (Id, ChannelId, StartTime, EndTime, Title, Description, EpisodeId, EpisodeNumber) VALUES (@id, @channelId, @startTime, @endTime, @title, @description, @episodeId, @episodeNumber)";

            const string cmdStr =
                @"INSERT INTO Program (Id, ChannelId, StartTime, EndTime, Title, Description, EpisodeId, EpisodeNumber) VALUES (@id, @channelId, @startTime, @endTime, @title, @description, @episodeId, @episodeNumber)";

            using (var cmd = (DbCommand) connection.CreateCommand())
            {
                cmd.CommandText = cmdStr;
                cmd.CommandType = CommandType.Text;
                
                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "id";
                idParam.DbType = DbType.AnsiString;
                idParam.Value = id.ToString();
                cmd.Parameters.Add(idParam);
                
                var channelIdParam = cmd.CreateParameter();
                channelIdParam.ParameterName = "channelId";
                channelIdParam.DbType = DbType.AnsiString;
                channelIdParam.Value = (object)channel ?? DBNull.Value;
                cmd.Parameters.Add(channelIdParam);
                
                var startParam = cmd.CreateParameter();
                startParam.ParameterName = "startTime";
                startParam.DbType = DbType.DateTime;
                startParam.Value = startTime;
                cmd.Parameters.Add(startParam);
                
                var endTimeParam = cmd.CreateParameter();
                endTimeParam.ParameterName = "endTime";
                endTimeParam.DbType = DbType.DateTime;
                endTimeParam.Value = endTime;
                cmd.Parameters.Add(endTimeParam);
                
                var titleParam = cmd.CreateParameter();
                titleParam.ParameterName = "title";
                titleParam.DbType = DbType.AnsiString;
                titleParam.Value = (object)title ?? DBNull.Value;
                cmd.Parameters.Add(titleParam);
                
                var descriptionParam = cmd.CreateParameter();
                descriptionParam.ParameterName = "description";
                descriptionParam.DbType = DbType.AnsiString;
                descriptionParam.Value = (object)description ?? DBNull.Value;
                cmd.Parameters.Add(descriptionParam);
                
                var episodeIdParam = cmd.CreateParameter();
                episodeIdParam.ParameterName = "episodeId";
                episodeIdParam.DbType = DbType.AnsiString;
                episodeIdParam.Value = (object)episodeId ?? DBNull.Value;
                cmd.Parameters.Add(episodeIdParam);
                
                var episodeNumParam = cmd.CreateParameter();
                episodeNumParam.ParameterName = "episodeNumber";
                episodeNumParam.DbType = DbType.AnsiString;
                episodeNumParam.Value = (object)episodeNum ?? DBNull.Value;
                cmd.Parameters.Add(episodeNumParam);
                
                cmd.ExecuteNonQuery();
            }

        }

        public static void WriteCategory(IDbConnection connection, Guid id, string category)
        {
            const string cmdStr = @"INSERT INTO ProgramCategories (Id, Category) VALUES (@id, @category)";
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = cmdStr;
                
                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "id";
                idParam.DbType = DbType.AnsiString;
                idParam.Value = id.ToString();
                cmd.Parameters.Add(idParam);

                var categoryParam = cmd.CreateParameter();
                categoryParam.ParameterName = "category";
                categoryParam.DbType = DbType.AnsiString;
                categoryParam.Value = category;
                cmd.Parameters.Add(categoryParam);

                cmd.ExecuteNonQuery();
            }
        }

        public static void WriteCredit(IDbConnection connection, Guid id, string role, string name)
        {
            const string cmdStr = @"INSERT INTO ProgramCredits (Id, Role, Name) VALUES (@id, @role, @name)";
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = cmdStr;

                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "id";
                idParam.DbType = DbType.AnsiString;
                idParam.Value = id.ToString();
                cmd.Parameters.Add(idParam);

                var roleParam = cmd.CreateParameter();
                roleParam.ParameterName = "role";
                roleParam.DbType = DbType.AnsiString;
                roleParam.Value = role;
                cmd.Parameters.Add(roleParam);

                var nameParam = cmd.CreateParameter();
                nameParam.ParameterName = "name";
                nameParam.DbType = DbType.AnsiString;
                nameParam.Value = name;
                cmd.Parameters.Add(nameParam);
                
                cmd.ExecuteNonQuery();
            }
        }

        private const string AttachName = "asldkqwer";

        public static void ExportToFile(DbConnection memory, string fileDbPath)
        {
            if (!File.Exists(fileDbPath))
                CreateSchema(string.Format("Data Source={0};Version=3;New=True;", fileDbPath),
                             "System.Data.SQLite");

            DataTable dt = memory.GetSchema("Tables");
            var tables = (from DataRow row in dt.Rows
                          select (string) row["TABLE_NAME"]).ToArray();
            AttachDatabase(memory, fileDbPath);
            foreach (var table in tables)
            {
                CopyTableData(memory, table, string.Format("{0}.{1}", AttachName, table));
            }
            DetachDatabase(memory);
        }

        private static void AttachDatabase(DbConnection memory, string fileDbPath)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("ATTACH '{0}' AS {1}", fileDbPath, AttachName);
            cmd.ExecuteNonQuery();
        }

        private static void CopyTableData(DbConnection memory, string source, string destination)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}",
                                            destination, source);
            cmd.ExecuteNonQuery();
        }

        private static void DetachDatabase(DbConnection memory)
        {
            var cmd = memory.CreateCommand();
            cmd.CommandText = string.Format("DETACH {0}", AttachName);
            cmd.ExecuteNonQuery();
        }

    }
}

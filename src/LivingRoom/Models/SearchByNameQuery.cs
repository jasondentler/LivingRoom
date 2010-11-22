using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using LivingRoom.XmlTv;

namespace LivingRoom.Models
{
    public class SearchByNameQuery
    {

        public static IEnumerable<Program> Query(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
                return new Program[] {};

            Mapper.CreateMap<IDataRecord, Program>()
                .ForMember(p => p.Channel,
                           mo => mo.MapFrom(r =>
                                            new Channel()
                                                {
                                                    Id = r.GetString(1),
                                                    Number = r.GetInt64(2),
                                                    ShortName = r.GetString(3),
                                                    LongName = r.GetString(4),
                                                    Icon =
                                                        !r.IsDBNull(5) && !string.IsNullOrEmpty(r.GetString(5))
                                                            ? "Channel/Icon/" + r.GetString(1)
                                                            : ""
                                                }))
                .ForMember(p => p.Attributes,
                           mo => mo.MapFrom(r =>
                                            r.IsDBNull(13)
                                                ? new string[] {}
                                                : r.GetString(13).Split(',')
                                     ));
            const string cmdStr =
@"SELECT 
	P.Id, 
	P.ChannelId,
	C.Channel AS ChannelNumber,
	C.ShortName AS ChannelShortName,
	C.LongName AS ChannelLongName,
    C.IconUrl AS ChannelIcon,
	P.StartTime,
	P.EndTime, 
	P.Title, 
    P.EpisodeTitle,
	P.Description, 
	P.EpisodeId, 
	P.EpisodeNumber,
    P.Attributes
FROM Program P
INNER JOIN Channel C 
ON P.ChannelId = C.Id
WHERE 
(P.Title LIKE @name) AND
P.EndTime > datetime('now', 'localtime')
ORDER BY P.StartTime ASC, C.Channel DESC
LIMIT 5";
            IEnumerable<Program> retval = null;
            using (var conn = Open())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdStr;
                    var nameParam = cmd.CreateParameter();
                    nameParam.ParameterName = "name";
                    nameParam.DbType = DbType.AnsiString;
                    nameParam.Value = "%" + name + "%";
                    cmd.Parameters.Add(nameParam);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        retval = Mapper.Map<IDataReader, IEnumerable<Program>>(rdr).ToArray();
                        rdr.Close();
                    }
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